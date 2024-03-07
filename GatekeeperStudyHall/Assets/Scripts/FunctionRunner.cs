using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FunctionRunner : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent events;
}


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