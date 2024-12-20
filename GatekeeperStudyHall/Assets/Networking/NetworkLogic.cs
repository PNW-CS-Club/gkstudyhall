using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkLogic : NetworkBehaviour
{
    /// An event this class invokes with a message whenever it has something it wants to say
    public event Action<string> OnLog;
    
    /// An event this class invokes after it handles any <see cref="NetworkManager.OnConnectionEvent"/>.
    /// Invoked with all the parameters from OnConnectionEvent.
    public event Action<NetworkManager, ConnectionEventData> AfterConnectionEvent;
    
    /// The last IP the NetworkManager has attempted to host from or connect to
    public string Ip { get; private set; }
    
    UnityTransport transport;
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        NetworkManager.Singleton.OnConnectionEvent += RespondToConnectionEvent;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        // unsubscribe from the event because the event will continue to trigger in the next scene
        NetworkManager.Singleton.OnConnectionEvent -= RespondToConnectionEvent;
    }


    /// Runs on all clients when called. Cleans up and loads the next scene in order to start the game.
    [Rpc(SendTo.ClientsAndHost)]
    public void StartGame_Rpc()
    {
        OnLog?.Invoke("Trying to start the game!");
        _ = SceneManager.LoadSceneAsync("CharSelectScene");
    }


    /// Sets the IP to this computer's local IP and tells the NetworkManager to try to start the host.
    public void StartHost()
    {
        Ip = GetLocalIPAddress();
        if (Ip == null)
        {
            OnLog?.Invoke("Could not find local IP address.");
            return;
        }
        
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        OnLog?.Invoke(wasSuccessful ? "Successfully started host" : "Could not start host");
    }
    
    /// Sets the IP to the given IP and tells the NetworkManager to try to start a client.
    /// <returns>whether the client started successfully</returns>
    public bool StartClient(string inputIp)
    {
        Ip = inputIp;
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        OnLog?.Invoke(wasSuccessful ? "Started client, trying to connect..." : "Could not start client");
        return wasSuccessful;
    }

    /// Stops the host or client. If this is the host, it first disconnects all other connected clients.
    public void Shutdown()
    {
        if (IsHost)
        {
            var hostId = NetworkManager.Singleton.LocalClientId;
            List<ulong> clientIds = NetworkManager.Singleton.ConnectedClientsIds.ToList();
            foreach (var id in clientIds)
            {
                if (id != hostId)
                    NetworkManager.Singleton.DisconnectClient(id, "Manual server shutdown");
            }
        }

        NetworkManager.Singleton.Shutdown();
        OnLog?.Invoke("Shut down connection");
    }
    
    
    /// Finds the local IPv4 address of this computer. 
    /// <returns>The local IPv4 address, or null if no network adapters could be found on this computer.</returns>
    private string GetLocalIPAddress() 
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) 
                return ipAddress.ToString();
        
        OnLog?.Invoke("No network adapters with an IPv4 address in the system!");
        return null;
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
                if (!IsHost)
                {
                    // happens if this computer disconnects as a non-host client
                    var reason = NetworkManager.Singleton.DisconnectReason;
                    if (!string.IsNullOrEmpty(reason)) 
                        OnLog?.Invoke($"Disconnect reason: {reason}");
                }
                break;
            }
            
            case ConnectionEvent.PeerConnected:
            case ConnectionEvent.PeerDisconnected:
            case ConnectionEvent.ClientConnected: 
                break;
        }

        // allows any objects with a reference to this object to respond to the connection event
        // AFTER this object has responded to it
        AfterConnectionEvent?.Invoke(nwm, data);
    }
}
