using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDiceRoll : DiceRoll
{
    public override void TakeOwnership(NetworkObjectReference netDiceRef)
    {
        if (NetworkPlayer.ownedInstance.player.isUp)
        {
            NetworkRoot.Instance.netLogic.TakeDiceOwnership_ServerRpc(netDiceRef);
        }
    }
}
