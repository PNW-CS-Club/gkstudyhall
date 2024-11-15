using System;
using System.Collections.Generic;
using UnityEngine;

public enum GateColor 
{
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}


/// <summary>
/// Represents a gate with its color and health.
/// </summary>
[CreateAssetMenu(fileName = "New_GateSO", menuName = "Scriptable Objects/GateSO")]
public class GateSO : ScriptableObject
{
    public const int STARTING_HEALTH = 6;
    public const int MAX_HEALTH = 6;

    public GateColor Color { get => color; }
    [SerializeField] GateColor color;

    public int health = STARTING_HEALTH;
    
    
    void OnEnable() 
    {
        health = STARTING_HEALTH;
    }

    public override string ToString() {
        return $"GateSO[health=\"{health}\", color=\"{color}\"]";
    }
}