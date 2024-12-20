using Unity.Netcode;
using UnityEngine;

public class TestRpcs : NetworkBehaviour
{
    int pingCounter = 0;
    
    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void Ping_Rpc(int pingCount)
    {
        // Receive ping from client
        // Send pong to ALL clients from the server
        Pong_Rpc(pingCount, "PONG!");
    }

    
    [Rpc(SendTo.ClientsAndHost)]
    void Pong_Rpc(int pingCount, string message)
    {
        // Receive pong from server
        Debug.Log($"Received pong from server for ping {pingCount} and message {message}");
    }
    
    void Update()
    {
        if (IsClient && Input.GetKeyDown(KeyCode.P))
        {
            // Send ping to server from a client
            Ping_Rpc(pingCounter);
            pingCounter++;
        }
    }
}