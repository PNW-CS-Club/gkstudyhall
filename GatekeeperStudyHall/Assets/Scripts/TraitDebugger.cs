using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TraitDebugger : MonoBehaviour {
    public TraitHandlerSO traitHandler = null;
    public PlayerSO playerInfo = null;
    public int roll = 1;
}


#if UNITY_EDITOR
[CustomEditor(typeof(TraitDebugger))]
public class TraitDebuggerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();

        TraitDebugger dbg = (TraitDebugger)target;

        if (dbg.roll < 1) { dbg.roll = 1; }
        if (dbg.roll > 4) { dbg.roll = 4; }

        if (dbg.playerInfo != null
            && dbg.traitHandler != null
            && GUILayout.Button($"traitHandler.ActivateTrait(<{dbg.playerInfo.name}>, {dbg.roll})")) {
            dbg.traitHandler.ActivateTrait(dbg.playerInfo, dbg.roll);
        }
    }
}
#endif
