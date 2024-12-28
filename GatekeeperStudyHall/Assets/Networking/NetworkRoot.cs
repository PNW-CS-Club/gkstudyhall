using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRoot : MonoBehaviour
{
    public List<NetworkPlayer> netPlayers = new();
    public NetworkLogic netLogic;

    char playerNameSuffix = 'A';

    void Start()
    {
        gameObject.name = "Network Root Obj";
        DontDestroyOnLoad(gameObject);
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
