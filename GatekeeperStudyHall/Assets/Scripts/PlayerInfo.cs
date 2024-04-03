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

    // public Card? PlayerCard { get; private set; } // to be implemented
    public int Health { get; private set; } = 10;
    public int Stockades { get; private set; } = 0;
}
