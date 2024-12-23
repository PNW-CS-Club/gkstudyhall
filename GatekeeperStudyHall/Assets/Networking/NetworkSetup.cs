using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSetup : MonoBehaviour
{
    [SerializeField] NetworkObject networkRootPrefab;
    [HideInInspector] public NetworkObject networkRootInstance;

    public event Action<NetworkObject> OnRootSpawned; 

    void Start()
    {
        print(NetworkManager.Singleton.SpawnManager);
        networkRootInstance = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(networkRootPrefab);
        OnRootSpawned?.Invoke(networkRootInstance);
    }
}