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
    
    
    // NOTE from: https://docs-multiplayer.unity3d.com/netcode/current/basics/networkbehavior/#spawning
    // For in-scene placed NetworkObjects, the OnNetworkSpawn method is invoked after the Start method
    // For NetworkObjects instantiated during runtime, the OnNetworkSpawn method is invoked before the Start method is invoked

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            username.Value = SystemInfo.deviceName;
            //Debug.Log($"set NetworkPlayer name to {username.Value.ToString()}");
        }
    }

    void Start()
    {
        var root = GameObject.FindWithTag("Network Root");

        if (root == null)
        {
            Debug.LogError("Could not find Network Root to be this object's parent!");
            return;
        }
        
        root.GetComponent<NetworkRoot>().AddPlayer(this);
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