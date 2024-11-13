using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the center gate and its health.
/// </summary>
[CreateAssetMenu(fileName = "New_CenterGateSO", menuName = "Scriptable Objects/CenterGateSO")]
public class CenterGateSO : ScriptableObject
{
    public const int MAX_HEALTH = 10;
    public int health = MAX_HEALTH;
    
    void OnEnable() {
        health = MAX_HEALTH;
    }

    public override string ToString() {
        return $"CenterGateSO[health=\"{health}\"]";
    }
}