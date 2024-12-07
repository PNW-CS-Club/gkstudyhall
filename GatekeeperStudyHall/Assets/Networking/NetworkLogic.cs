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
    
    public bool BecomeHost()
    {
        Ip = GetLocalIPAddress();
        if (Ip == null) return false;
        
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        OnLog?.Invoke($"Successfully became host? {wasSuccessful}");
        if (!wasSuccessful) return false;

        SetNetworkRole(NetworkRole.Host);
        return true;
    }
    
    public bool BecomeClient(string inputIp)
    {
        Ip = inputIp;
        transport.ConnectionData.Address = Ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        OnLog?.Invoke($"Successfully became client? {wasSuccessful}");
        if (!wasSuccessful) return false;

        SetNetworkRole(NetworkRole.Client);
        return true;
    }

    public void Shutdown()
    {
        NetworkManager.Singleton.Shutdown();
        OnLog?.Invoke("Shut down connection");
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
