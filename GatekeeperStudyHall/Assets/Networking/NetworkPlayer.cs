using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [HideInInspector] 
    public NetworkVariable<FixedString64Bytes> username = new(
        "no name", 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner
    );

    bool isInitialized = false;

    void Initialize()
    {
        if (IsOwner)
        {
            username.Value = SystemInfo.deviceName;
            Debug.Log($"set NetworkPlayer name to {username.Value.ToString()}");
        }

        isInitialized = true;
        Debug.Log("initialized NetworkPlayer");
    }

    public override void OnNetworkSpawn()
    {
        if (!isInitialized) Initialize();
    }

    void Start()
    {
        if (!isInitialized) Initialize();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(NetworkPlayer))]
public class NetworkPlayerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkPlayer netPlayer = (NetworkPlayer)target;
        EditorGUILayout.LabelField($"Username: {netPlayer.username.Value.ToString()}");
    }
}
#endif