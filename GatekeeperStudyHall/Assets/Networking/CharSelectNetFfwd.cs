using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectNetFfwd : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] List<PlayerSO> playerSOs;
    [SerializeField] List<CardSO> cardSOs;
    
    void Awake()
    {
        if (Globals.multiplayerType == MultiplayerType.Offline) return;

        var players = playerListSO.list;
        players.Clear();

        var netPlayers = NetworkRoot.Instance.netPlayers;
        for (int i = 0; i < netPlayers.Count; i++)
        {
            players.Add(playerSOs[i]);
            playerSOs[i].card = cardSOs[i];
            netPlayers[i].player = playerSOs[i];
        }
        
        SceneManager.LoadScene("GkScene");
    }
}
