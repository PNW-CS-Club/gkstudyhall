using System;
using System.Collections.Generic;
using UnityEngine;

public enum GateColor 
{
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}


/// <summary>
/// Represents a gate with its color and health. Provides methods to take damage, heal, and reset after breaking.
/// </summary>
[CreateAssetMenu(fileName = "New_GateSO", menuName = "Scriptable Objects/GateSO")]
public class GateSO : ScriptableObject
{
    const int STARTING_HEALTH = 6;
    const int MAX_HEALTH = 6;

    public GateColor Color => color;
    [SerializeField] GateColor color;

    public int Health => health;
    [SerializeField] int health = STARTING_HEALTH;
    
    
    void OnEnable() 
    {
        health = STARTING_HEALTH;
    }

    public override string ToString() {
        return $"GateSO[health=\"{health}\", color=\"{color}\"]";
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        
        health = Mathf.Max(0, health - damage);
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;
        
        health = Mathf.Min(health + amount, MAX_HEALTH);
    }

    public void Reset()
    {
        health = STARTING_HEALTH;
    }
}