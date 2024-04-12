using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TraitDebugger : MonoBehaviour { }


#if UNITY_EDITOR
[CustomEditor(typeof(TraitDebugger))]
public class TraitDebuggerEditor : Editor
{
    // these are for keeping track of state in the immediate mode gui
    // (i.e. our custom editor fields don't store variables so we have to do it ourselves)
    PlayerSO playerInfo = null;
    int roll = 1;

    public override void OnInspectorGUI() 
    {
        playerInfo = (PlayerSO)EditorGUILayout.ObjectField("Player", playerInfo, typeof(PlayerSO), false);

        roll = EditorGUILayout.IntField("Roll", roll);
        if (roll < 1) { roll = 1; }
        if (roll > 4) { roll = 4; }

        if (playerInfo != null && GUILayout.Button($"TraitHandler.ActivateTrait(<{playerInfo.name}>, {roll})")) {
            TraitHandler.ActivateTrait(playerInfo, roll);
        }
    }
}
#endif
