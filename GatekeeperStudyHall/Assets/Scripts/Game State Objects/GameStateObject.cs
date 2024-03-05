using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows us to create GameStateObjects in the Project tab's create menu
[CreateAssetMenu(fileName = "New GameStateObject", menuName = "GameStateObject")]
/*
This scriptable object is meant to just hold an enum so that 
we can pass enums into event functions indirectly by passing 
the scriptable object in and then reading its enum value.
*/
public class GameStateObject : ScriptableObject
{
    public GameState gameState;
}
