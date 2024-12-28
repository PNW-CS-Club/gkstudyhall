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
    [HideInInspector] public NetworkObject networkRootInstance;

    /// An event this class invokes with a message whenever it has something it wants to say
    public event Action<string> OnLog;
    
    /// An event this class invokes with the root NetworkObject when it is first created
    public event Action<NetworkObject> OnRootSpawned; 
    
    /// The last IP the NetworkManager has attempted to host from or connect to
    public string HostIp { get; private set; }

    NetworkManager nwm;
    UnityTransport transport;

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
        OnLog?.Invoke(wasSuccessful ? "Successfully started host" : "Could not start host");

        // only spawn the root on the host (other clients will get it automatically when they connect)
        // TODO: SpawnRoot might be bad to call repeatedly
        if (wasSuccessful) SpawnRoot();

        return wasSuccessful;
    }

    /// Sets the IP to the given IP and tells the NetworkManager to try to start a client.
    /// <returns>whether the client started successfully</returns>
    public bool StartClient(string inputIp)
    {
        HostIp = inputIp;
        transport.ConnectionData.Address = HostIp;
        
        bool wasSuccessful = nwm.StartClient();
        OnLog?.Invoke(wasSuccessful ? "Started client, trying to connect..." : "Could not start client");
        
        return wasSuccessful;
    }


    public void ShutdownClient()
    {
        NetworkManager.Singleton.Shutdown();
        OnLog?.Invoke("Shut down connection");
    }
    
    
    /// Finds the local IPv4 address of this computer. 
    /// <returns>The local IPv4 address, or null if no network adapters could be found on this computer.</returns>
    private string GetLocalIPAddress() 
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork) 
                return ipAddress.ToString();
        
        OnLog?.Invoke("Failed to find local IP address: No network adapters with an IPv4 address in the system!");
        return null;
    }
    
    private void SpawnRoot() 
    {
        print(nwm.SpawnManager);
        // NOTE from: https://www.reddit.com/r/Unity3D/comments/xpig05/comment/j3lrqdx/
        // While NetworkManager.Singleton indeed exists, it has to be running for SpawnManager to exist.
        // You do that by starting server by either NetworkManager.Singleton.StartServer() or by NetworkManager.Singleton.StartHost().
        
        networkRootInstance = nwm.SpawnManager.InstantiateAndSpawn(networkRootPrefab);
        OnRootSpawned?.Invoke(networkRootInstance);
    }
}