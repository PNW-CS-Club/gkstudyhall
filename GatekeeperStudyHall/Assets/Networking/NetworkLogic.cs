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
    public event Action<string> OnLog;
    public event Action<NetworkManager, ConnectionEventData> OnConnectionEvent;

    public string Ip { get; private set; }
    
    UnityTransport transport;
    public List<String> usernames = new(); 
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.OnTransportEvent += LogTransportEvent;
        
        NetworkManager.Singleton.OnConnectionEvent += RespondToConnectionEvent;
    }

    void LogTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
    {
        OnLog?.Invoke($"OnTransportEvent: \n  eventType: {eventType}\n  clientId: {clientId}\n  payload: {payload}\n  receiveTime: {receiveTime}");
    }
    
    
    [Rpc(SendTo.ClientsAndHost)]
    public void StartGame_Rpc()
    {
        OnLog?.Invoke("Trying to start the game!");
        _ = SceneManager.LoadSceneAsync("CharSelectScene");
    }


    public void StartHost()
    {
        Ip = GetLocalIPAddress();
        if (Ip == null) return;
        
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        OnLog?.Invoke(wasSuccessful ? "Successfully started host" : "Could not start host");
    }
    
    public void StartClient(string inputIp)
    {
        Ip = inputIp;
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        OnLog?.Invoke(wasSuccessful ? "Started client, trying to connect..." : "Could not start client");
    }

    public void Shutdown()
    {
        if (NetworkManager.Singleton.IsHost)
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
    
    private void UpdateUsernames()
    {
        usernames = GameObject.FindGameObjectsWithTag("NetPlayer")
            .Select(go => go.GetComponent<NetworkPlayer>().username.Value.ToString())
            .ToList();
    }
    
    
    private string GetLocalIPAddress() 
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) 
                return ipAddress.ToString();
        
        OnLog?.Invoke("No network adapters with an IPv4 address in the system!");
        return null;
    }

    private void LogClientDisconnect()
    {
        var reason = NetworkManager.Singleton.DisconnectReason;
        if (!string.IsNullOrEmpty(reason)) 
            OnLog?.Invoke($"Disconnect reason: {reason}");
    }

    private void RespondToConnectionEvent(NetworkManager nwm, ConnectionEventData data)
    {
        if (!IsClient) return;
        
        switch (data.EventType)
        {
            case ConnectionEvent.ClientDisconnected:
                LogClientDisconnect();
                break;
            
            case ConnectionEvent.PeerConnected:
            case ConnectionEvent.PeerDisconnected:
                UpdateUsernames();
                break;
            
            case ConnectionEvent.ClientConnected:
                break;
        }

        OnConnectionEvent?.Invoke(nwm, data);
    }
}
