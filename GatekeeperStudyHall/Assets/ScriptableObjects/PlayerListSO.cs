using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PlayerListSO", menuName = "Scriptable Objects/PlayerListSO")]
public class PlayerListSO : ScriptableObject
{
    public List<PlayerSO> list;
}
