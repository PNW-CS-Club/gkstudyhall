using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public static readonly int STARTING_HEALTH = 10;
    public static readonly int MAX_HEALTH = 12;

    // These are regular fields so that we can inspect them in the editor.
    // Using a boolean for the stockade because it doesn't seem like we'll use the multiple stockade rule.
    public CardData card;
    public int health = STARTING_HEALTH;
    public bool hasStockade = false;

    public bool isAlive = true;

    // not final
    public bool doubleDamageToCenter = false;
    public bool doubleDamageToSelf = false;
}
