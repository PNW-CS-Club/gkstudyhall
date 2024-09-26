using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// A simple debug class that has a button in the inspector. 
/// When the button is clicked, it activates the specified trait of the specified player.
/// </summary>
public class TraitDebugger : MonoBehaviour {
    public TraitHandlerSO traitHandler = null;
    public PlayerSO player = null;
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

        dbg.roll = Mathf.Clamp(dbg.roll, 1, 4);

        if (dbg.player != null
            && dbg.traitHandler != null
            && GUILayout.Button($"traitHandler.ActivateTrait(<{dbg.player.name}>, {dbg.roll})")) 
        {
            dbg.traitHandler.ActivateTrait(dbg.player, dbg.roll);
        }
    }
}
#endif
