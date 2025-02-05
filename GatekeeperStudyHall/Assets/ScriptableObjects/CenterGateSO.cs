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
    public const int STARTING_HEALTH = 10;
    
    public int Health => health;
    [SerializeField] int health = STARTING_HEALTH;
    
    void OnEnable() {
        health = STARTING_HEALTH;
    }

    public override string ToString() {
        return $"CenterGateSO[health=\"{health}\"]";
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
}