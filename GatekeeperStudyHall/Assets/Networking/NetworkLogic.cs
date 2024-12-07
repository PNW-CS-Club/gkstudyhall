using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class NetworkLogic : MonoBehaviour
{
    public event Action<string> OnLog;
    public event Action OnClientDisconnect;
    public event Action<bool> OnClientConnect;

    public string Ip { get; private set; }
    
    UnityTransport transport;
    
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        NetworkManager.Singleton.OnClientStopped += _ => OnClientDisconnect?.Invoke();
        NetworkManager.Singleton.OnClientConnectedCallback += _ => OnClientConnect?.Invoke(NetworkManager.Singleton.IsHost);
        
        OnClientDisconnect += LogClientDisconnect;
    }
    
    
    public void BecomeHost()
    {
        Ip = GetLocalIPAddress();
        if (Ip == null) return;
        
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        OnLog?.Invoke(wasSuccessful ? "Successfully became host" : "Could not become host");
    }
    
    public void BecomeClient(string inputIp)
    {
        Ip = inputIp;
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        OnLog?.Invoke(wasSuccessful ? "Successfully became client" : "Could not become client");
    }

    public void Shutdown()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            List<ulong> clientIds = NetworkManager.Singleton.ConnectedClients
                .Select(x => x.Key)
                .ToList();
            foreach (var id in clientIds)
                NetworkManager.Singleton.DisconnectClient(id, "Manual server shutdown");
        }
        
        NetworkManager.Singleton.Shutdown();
        OnLog?.Invoke("Shut down connection");
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
}
