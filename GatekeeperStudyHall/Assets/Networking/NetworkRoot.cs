using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkRoot : MonoBehaviour
{
    /// The canonical instance of this class (more than one cannot exist simultaneously)
    public static NetworkRoot Instance;

    public int PlayerCount => transform.childCount;
    public List<NetworkPlayer> netPlayers = new();
    public NetworkLogic netLogic;
    
    char playerNameSuffix = 'A';
    
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A NetworkRoot component already exists! Destroying self...");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        gameObject.name = "Network Root Obj";
        DontDestroyOnLoad(gameObject);
    }

    public static void GkAwake(NetworkObject networkDicePrefab, Vector3 diceSpawnLocation)
    {
        if (Instance == null) return;

        Instance.netLogic.CreateNetworkDice(networkDicePrefab, diceSpawnLocation);
    } 
    

    public void AddPlayer(NetworkPlayer player)
    {
        player.transform.SetParent(transform);
        netPlayers.Add(player);
        player.gameObject.name = "Net Player " + playerNameSuffix;
        playerNameSuffix++;
    }

    void Update()
    {
        // remove outdated netPlayer references
        for (int i = 0; i < netPlayers.Count; i++)
        {
            if (netPlayers[i] == null)
            {
                netPlayers.RemoveAt(i);
                i--;
            }
        }
    }
}
