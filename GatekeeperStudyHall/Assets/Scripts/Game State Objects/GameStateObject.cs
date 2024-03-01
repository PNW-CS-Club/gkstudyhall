using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes a new entry in asset creation menu (in Project tab)
[CreateAssetMenu(fileName = "New GameStateObject", menuName = "GameStateObject")]
public class GameStateObject : ScriptableObject
{
    // This scriptable object is meant to just hold an enum
    // so that we can pass enums into event functions indirectly
    // by passing the scriptable object in and then reading its enum value.
    public GameState gameState;
}
