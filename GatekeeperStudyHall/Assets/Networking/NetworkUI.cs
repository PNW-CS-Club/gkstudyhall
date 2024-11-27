using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public enum NetworkRole
{
    None, Client, Host
}


public class NetworkUI : MonoBehaviour
{
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_Text ipDisplay;

    string ip;
    UnityTransport transport;
    NetworkRole role = NetworkRole.None;
    ulong clientID = 0;
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.OnClientConnectedCallback += SetClientID;
    }
    
    public void BecomeHost()
    {
        ip = GetLocalIPAddress();
        ipDisplay.text = ip;
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        role = NetworkRole.Host;
    }
    
    public void BecomeClient()
    {
        ip = ipInput.text;
        transport.ConnectionData.Address = ip;
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        role = NetworkRole.Client;
    }

    public void DisconnectClient()
    {
        if (role == NetworkRole.Host) return;
        
        NetworkManager.Singleton.DisconnectClient(clientID);
        role = NetworkRole.None;
    }
    
    private string GetLocalIPAddress() 
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) 
                return ipAddress.ToString();
        
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    private void SetClientID(ulong id)
    {
        if (role != NetworkRole.Host)
        {
            clientID = id;
        }
        
        Debug.Log($"Call to SetClientID with ID: {id}");
    }
}
