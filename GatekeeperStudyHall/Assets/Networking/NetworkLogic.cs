using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class NetworkLogic : MonoBehaviour
{
    public event Action<string> OnLog;
    public event Action OnClientDisconnect;
    public event Action<bool> OnClientConnect;
    
    public NetworkVariable<FixedString64Bytes> username1 = new("username1");
    public NetworkVariable<FixedString64Bytes> username2 = new("username2");
    public NetworkVariable<FixedString64Bytes> username3 = new("username3");
    public NetworkVariable<FixedString64Bytes> username4 = new("username4");

    public string Ip { get; private set; }
    
    UnityTransport transport;
    
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.OnTransportEvent += LogTransportEvent;
        
        NetworkManager.Singleton.OnClientConnectedCallback 
            += _ => OnClientConnect?.Invoke(NetworkManager.Singleton.IsHost);
        NetworkManager.Singleton.OnClientStopped 
            += _ => OnClientDisconnect?.Invoke();
        
        OnClientDisconnect += LogClientDisconnect;
    }

    void LogTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
    {
        OnLog?.Invoke($"OnTransportEvent: \n  eventType: {eventType}\n  clientId: {clientId}\n  payload: {payload}\n  receiveTime: {receiveTime}");
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
            List<ulong> clientIds = NetworkManager.Singleton.ConnectedClientsIds.ToList();
            foreach (var id in clientIds)
                NetworkManager.Singleton.DisconnectClient(id, "Manual server shutdown");
        }
        
        NetworkManager.Singleton.Shutdown();
        OnLog?.Invoke("Shut down connection");
    }


    

    public List<string> GetUsernames()
    {
        if (NetworkManager.Singleton.IsHost)
            UpdateUsernames();

        return new List<string>
        {
            username1.Value.ToString(),
            username2.Value.ToString(),
            username3.Value.ToString(),
            username4.Value.ToString(),
        };
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
    
    private void UpdateUsernames()
    {
        var list = NetworkManager.Singleton.ConnectedClientsList
            .Select(x => x.PlayerObject.GetComponent<NetworkPlayer>().username.Value.ToString())
            .ToList();

        username1.Value = list.Count >= 1 ? list[0] : "username1";
        username2.Value = list.Count >= 2 ? list[1] : "username2";
        username3.Value = list.Count >= 3 ? list[2] : "username3";
        username4.Value = list.Count >= 4 ? list[3] : "username4";
    }
}
