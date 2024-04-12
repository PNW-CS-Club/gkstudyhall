using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Scriptable Objects/Player")]
public class PlayerSO : ScriptableObject
{
    public static readonly int STARTING_HEALTH = 10;
    public static readonly int MAX_HEALTH = 12;

    // These are regular fields so that we can inspect them in the editor.
    // Using a boolean for the stockade because it doesn't seem like we'll use the multiple stockade rule.
    public CardSO card;
    public int health;
    public bool hasStockade;

    public bool isAlive;

    // not final
    public bool doubleDamageToCenter;
    public bool doubleDamageToSelf;


    // when this SO is loaded into a scene, reset values that may have been changed
    private void OnEnable() {
        health = STARTING_HEALTH;
        hasStockade = false;

        isAlive = true;

        doubleDamageToCenter = false;
        doubleDamageToSelf = false;
    }
}
