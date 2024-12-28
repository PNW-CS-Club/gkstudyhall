using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkLogic : NetworkBehaviour
{
    [SerializeField] NetworkSetup netSetup;
    /// An event this class invokes with a message whenever it has something it wants to say
    public event Action<string> OnLog;
    
    /// An event this class invokes after it handles any <see cref="NetworkManager.OnConnectionEvent"/>.
    /// Invoked with all the parameters from OnConnectionEvent.
    public event Action<NetworkManager, ConnectionEventData> AfterConnectionEvent;

    NetworkManager nwm;
    
    void Start()
    {
        // NetworkManager.Singleton is null sometimes (idk why ??) so we keep a reference to it in nwm 
        nwm = NetworkManager.Singleton;
        nwm.OnConnectionEvent += RespondToConnectionEvent;
    }

    public override void OnDestroy()
    {
        // unsubscribe from the event because the event will continue to trigger in the next scene
        nwm.OnConnectionEvent -= RespondToConnectionEvent;
        
        base.OnDestroy();
    }


    /// Runs on all clients when called. Cleans up and loads the next scene in order to start the game.
    [Rpc(SendTo.ClientsAndHost)]
    public void StartGame_Rpc()
    {
        OnLog?.Invoke("Trying to start the game!");
        _ = SceneManager.LoadSceneAsync("CharSelectScene");
    }

    // Confirm our connection
    [Rpc(SendTo.SpecifiedInParams)]
    public void ConfirmConnection_Rpc( RpcParams param = default ) {
        Debug.Log( "How does this work" );
        NetworkUI.Instance.ChangeUIState(NetworkUIState.Joining);
    }

    /// Stops the host after disconnecting all non-host clients.
    public void ShutdownHost()
    {
        var hostId = nwm.LocalClientId;
        List<ulong> clientIds = nwm.ConnectedClientsIds.ToList();
        foreach (var id in clientIds)
        {
            if (id != hostId)
                nwm.DisconnectClient(id, "Manual server shutdown");
        }

        nwm.Shutdown();
        OnLog?.Invoke("Shut down connection");
    }
    
    
    /// A method meant to receive the information from <see cref="NetworkManager.OnConnectionEvent"/>.
    /// <param name="nwm">this computer's NetworkManager</param>
    /// <param name="data">a struct that holds all the data pertaining to the event</param>
    private void RespondToConnectionEvent(NetworkManager nwm, ConnectionEventData data)
    {
        OnLog?.Invoke($"Received connection event: {data.EventType}");
        
        switch (data.EventType)
        {
            case ConnectionEvent.ClientDisconnected:
            {
                if (data.ClientId == nwm.LocalClientId)
                {
                    // happens if this computer is the one that disconnected
                    var reason = nwm.DisconnectReason;
                    if (!string.IsNullOrEmpty(reason)) 
                        OnLog?.Invoke($"Disconnect reason: {reason}");
                }
                break;
            }
            
            case ConnectionEvent.PeerConnected:
                break;
            case ConnectionEvent.PeerDisconnected:
                break;
            case ConnectionEvent.ClientConnected: 
                ConfirmConnection_Rpc( RpcTarget.Single( data.ClientId, RpcTargetUse.Temp ) );
                break;
        }

        // allows any objects with a reference to this object to respond to the connection event
        // AFTER this object has responded to it
        AfterConnectionEvent?.Invoke(nwm, data);
    }
}
