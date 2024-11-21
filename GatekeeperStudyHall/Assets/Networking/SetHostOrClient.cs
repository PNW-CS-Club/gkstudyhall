using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SetHostOrClient : MonoBehaviour
{
    NetworkManager m_NetworkManager;

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
        m_NetworkManager.StartHost();
    }

    public void SetClient()
    {
        m_NetworkManager.StartClient();
    }

    public void SetServer()
    {
        m_NetworkManager.StartServer();
    }
}
