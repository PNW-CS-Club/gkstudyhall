using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FunctionRunner : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent events;
}


#if UNITY_EDITOR
[CustomEditor(typeof(FunctionRunner))]
public class FunctionRunnerEditor : Editor
{
    // draws the defualt inspector and then a button beneath it that runs the functions
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Run Functions")) {
            FunctionRunner runner = (FunctionRunner)target;
            runner.events.Invoke();
        }
    }
}
#endif
