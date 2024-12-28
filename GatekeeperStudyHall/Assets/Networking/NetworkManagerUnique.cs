using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// This script just makes sure that there is only ever ONE NetworkManager GameObject
public class NetworkManagerUnique : MonoBehaviour
{
    // using Start instead of Awake bc nwm.singleton is not set yet on first awake
    void Start()
    {
        var myNwm = GetComponent<NetworkManager>();
        
        if (NetworkManager.Singleton != myNwm)
        {
            Destroy(gameObject);
        }
    }
}
