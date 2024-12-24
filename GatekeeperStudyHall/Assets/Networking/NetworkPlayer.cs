using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NetworkPlayer : NetworkBehaviour
{
    [HideInInspector] 
    public NetworkVariable<FixedString64Bytes> username = new(
        "no name", 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner
    );

    bool isInitialized = false;
    bool wasReparented = false;

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

    void TryReparent()
    {
        var root = GameObject.FindWithTag("Network Root");

        if (root == null)
        {
            Debug.LogWarning("Network Root not found this call...");
            return;
        }
        
        root.GetComponent<NetworkRoot>().AddPlayer(this);
        
        wasReparented = true;
        Debug.Log("Re-Parented NetworkPlayer");
    }

    public override void OnNetworkSpawn()
    {
        if (!isInitialized) Initialize();
        if (!wasReparented) TryReparent();
    }

    void Start()
    {
        if (!isInitialized) Initialize();
        if (!wasReparented) TryReparent();
    }

    void Update()
    {
        if (!wasReparented) TryReparent();
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