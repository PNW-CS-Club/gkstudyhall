using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollButtonAction : MonoBehaviour
{
    public void DiceRoll() {
        // Random.Range(int, int) includes the lower value and excludes the higher value,
        // so the range will be 1 - 6
        int rollOutcome = Random.Range(1,7);
        Debug.Log(rollOutcome);
    }
}
