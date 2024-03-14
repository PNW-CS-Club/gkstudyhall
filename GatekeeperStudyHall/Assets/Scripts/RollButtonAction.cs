using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollButtonAction : MonoBehaviour
{
    public void DiceRoll() {
        // Random.Range returns a float which is then truncated to an int so the range will be 1 - 6
        int rollOutcome = Random.Range(1,7);
        Debug.Log(rollOutcome);
    }
}
