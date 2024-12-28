using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class NetworkSetup : MonoBehaviour
{
    [SerializeField] NetworkObject networkRootPrefab;

    /// The canonical instance of this class (more than one cannot exist simultaneously)
    public static NetworkSetup Instance;
    
    /// The last IP the NetworkManager has attempted to host from or connect to
    public string HostIp { get; private set; }

    NetworkManager nwm;
    UnityTransport transport;
    
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A NetworkSetup component already exists! Destroying self...");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        // NetworkManager.Singleton is null sometimes (idk why ??) so we keep a reference to it in nwm 
        nwm = NetworkManager.Singleton;
        transport = nwm.GetComponent<UnityTransport>();
    }
    
    
    /// Sets the IP to this computer's local IP and tells the NetworkManager to try to start the host.
    public bool StartHost()
    {
        HostIp = GetLocalIPAddress();
        if (HostIp == null) return false;
        
        transport.ConnectionData.Address = HostIp;
        
        bool wasSuccessful = nwm.StartHost();
        Logger.Log(wasSuccessful ? "Successfully started host" : "Could not start host");

        // only spawn the root on the host (other clients will get it automatically when they connect)
        if (wasSuccessful)
            nwm.SpawnManager.InstantiateAndSpawn(networkRootPrefab);

        return wasSuccessful;
    }

    /// Sets the IP to the given IP and tells the NetworkManager to try to start a client.
    /// <returns>whether the client started successfully</returns>
    public bool StartClient(string inputIp)
    {
        HostIp = inputIp;
        transport.ConnectionData.Address = HostIp;
        
        bool wasSuccessful = nwm.StartClient();
        Logger.Log(wasSuccessful ? "Started client, trying to connect..." : "Could not start client");
        
        return wasSuccessful;
    }


    /// <summary>Simply shuts down the client.
    /// <para><i>It is important that this is not in a network object spawned by the host.
    /// The client network manager needs to be able to shut down even if it hasn't found a host to connect to
    /// (and therefore has not spawned the network object).</i></para></summary>
    public void ShutdownClient()
    {
        NetworkManager.Singleton.Shutdown();
        Logger.Log("Shut down connection");
    }
    
    
    /// Finds the local IPv4 address of this computer. 
    /// <returns>The local IPv4 address, or null if no network adapters could be found on this computer.</returns>
    private string GetLocalIPAddress() 
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) 
                return ipAddress.ToString();
        
        Logger.Log("Failed to find local IP address: No network adapters with an IPv4 address could be found on this computer!");
        return null;
    }
}