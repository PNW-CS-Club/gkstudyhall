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
    [SerializeField] TMP_Text debugDisplay;

    string ip;
    UnityTransport transport;
    NetworkRole role = NetworkRole.None;
    ulong clientID = 0;

    void SetNetworkRole(NetworkRole newRole)
    {
        role = newRole;
        AddDebugLine($"new role value: {newRole}");
    }
    
    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.OnClientConnectedCallback += SetClientID;
        debugDisplay.text = "";
    }
    
    public void BecomeHost()
    {
        ip = GetLocalIPAddress();
        if (ip == null) return;
        
        ipDisplay.text = ip;
        transport.ConnectionData.Address = ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartHost();
        AddDebugLine($"Successfully became host? {wasSuccessful}");
        if (!wasSuccessful) return;

        SetNetworkRole(NetworkRole.Host);
    }
    
    public void BecomeClient()
    {
        ip = ipInput.text;
        transport.ConnectionData.Address = ip;
        
        bool wasSuccessful = NetworkManager.Singleton.StartClient();
        AddDebugLine($"Successfully became client? {wasSuccessful}");
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
        
        AddDebugLine("No network adapters with an IPv4 address in the system!");
        return null;
    }

    private void SetClientID(ulong id)
    {
        if (role != NetworkRole.Host)
        {
            clientID = id;
        }
        
        AddDebugLine($"Call to SetClientID with ID: {id}");
    }

    private void AddDebugLine(string line) => debugDisplay.text += line + "\n";
}
