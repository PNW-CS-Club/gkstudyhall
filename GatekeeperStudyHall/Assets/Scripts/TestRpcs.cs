using Unity.Netcode;
using UnityEngine;

public class TestRpcs : NetworkBehaviour
{
    int pingCounter = 0;
    
    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void Ping_Rpc(int pingCount)
    {
        // Server -> Clients because PongRpc sends to NotServer
        // Note: This will send to all clients.
        // Sending to the specific client that requested the pong will be discussed in the next section.
        Pong_Rpc(pingCount, "PONG!");
    }

    
    [Rpc(SendTo.ClientsAndHost)]
    void Pong_Rpc(int pingCount, string message)
    {
        Debug.Log($"Received pong from server for ping {pingCount} and message {message}");
    }
    
    void Update()
    {
        if (IsClient && Input.GetKeyDown(KeyCode.P))
        {
            // Client -> Server because PingRpc sends to Server
            Ping_Rpc(pingCounter);
            pingCounter++;
        }
    }
}