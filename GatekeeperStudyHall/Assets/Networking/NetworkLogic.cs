using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public enum NetworkRole
{
    None, Client, Host
}


public class NetworkLogic : MonoBehaviour
{
    public event Action<string> OnLog;

    public string Ip { get; private set; }
    UnityTransport transport;
    NetworkRole role = NetworkRole.None;
    ulong clientID = 0;

    void SetNetworkRole(NetworkRole newRole)
    {
        role = newRole;
        OnLog?.Invoke($"new role value: {newRole}");
    }
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.OnClientConnectedCallback += SetClientID;
    }
    
    public void BecomeHost()
    {
        Ip = GetLocalIPAddress();
        if (Ip == null) return;
        
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        OnLog?.Invoke($"Successfully became host? {wasSuccessful}");
        if (!wasSuccessful) return;

        SetNetworkRole(NetworkRole.Host);
    }
    
    public void BecomeClient(string inputIp)
    {
        Ip = inputIp;
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        OnLog?.Invoke($"Successfully became client? {wasSuccessful}");
        if (!wasSuccessful) return;

        SetNetworkRole(NetworkRole.Client);
    }

    public void DisconnectClient()
    {
        // only server can disconnect clients, please use shutdown()
        
        if (role == NetworkRole.Host) return;
        
        NetworkManager.Singleton.DisconnectClient(clientID);
        SetNetworkRole(NetworkRole.None);
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

    private void SetClientID(ulong id)
    {
        if (role != NetworkRole.Host)
        {
            clientID = id;
        }
        
        OnLog?.Invoke($"Call to SetClientID with ID: {id}");
    }
}
