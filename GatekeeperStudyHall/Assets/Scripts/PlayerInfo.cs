using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerInfo : ScriptableObject
{
    /*
        Stores the following information:

        Player Card
        Health - Initialize to 10 HP
        Stockades - 0 (Player takes no damage when stockade >= 1)

        Maybe TurnOrder / Player Number (Could be handled elsewhere)
    */

    // public CardData PlayerCard { get; set; }
    public int Health { get; set; } = 10;
    public int Stockades { get; set; } = 0;
}
