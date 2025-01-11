using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDiceRoll : DiceRoll
{
    public override void ChangeOwnership()
    {
        if (NetworkPlayer.ownedInstance.player.isUp)
        {
            NetworkRoot.Instance.netLogic.ChangeDiceOwnership_ServerRpc();
        }
    }
}
