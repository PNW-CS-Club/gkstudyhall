using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class SetHostOrClient : MonoBehaviour
{
    NetworkManager m_NetworkManager;
    
    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;
    
    /* Gets the Ip Address of your connected network and
    shows on the screen in order to let other players join
    by inputing that Ip in the input field */
    // ONLY FOR HOST SIDE 
    public string GetLocalIPAddress() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
            	ipAddressText.text = ip.ToString();
            	ipAddress = ip.ToString();
            	return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    
    /* Sets the Ip Address of the Connection Data in Unity Transport
    to the Ip Address which was input in the Input Field */
    // ONLY FOR CLIENT SIDE
    public void SetIpAddress()
    {
        ipAddress = ip.text;
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }

    void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
    }

    public void Stop()
    {
        //m_NetworkManager.DisconnectClient();
    }
    
    public void SetHost() 
    {
        GetLocalIPAddress();
        m_NetworkManager.StartHost();
    }

    public void SetClient()
    {
        SetIpAddress();
        m_NetworkManager.StartClient();
    }

    public void SetServer()
    {
        m_NetworkManager.StartServer();
    }
}
