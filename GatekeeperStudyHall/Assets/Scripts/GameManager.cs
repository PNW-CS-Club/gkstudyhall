using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    int turn = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Access Player info script here if needed
        List<PlayerInfo> playerList = new List<PlayerInfo>
        {
            // loop to add players to list when scene starts
        };

        //amountOfPlayers variable
        //int amountOfPlayers = globals.cards.length();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Handle turn switching, needs amountOfPlayers not 4 once working;
    void turnSwitch()
    {
       /** do
        {
            turn = (turn + 1) % 4;
        } while (playerList[turn].isdead);
       **/
    }





}
