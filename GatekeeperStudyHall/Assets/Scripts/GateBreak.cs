using System.Collections.Generic;
using UnityEngine;

public class GateBreak : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] PlayerSelection playerSelect;
    GameManager gameManager;
    
    void Start()
    {
        GameObject gmObject = GameObject.FindWithTag("GameManager");
        gameManager = gmObject.GetComponent<GameManager>();
    }

    
    /// <summary>
    /// This should be called after this gate has been broken
    /// and a player has rolled even or odd.
    /// </summary>
    /// <returns>The next state the player should enter this turn, or null if the turn should end.</returns>
    public State? DoBreakEffect(PlayerSO player, GateSO gate, int roll) 
    {
        return (roll % 2 == 0) ? DoEvenEffect(player, gate, roll) : DoOddEffect(player, gate, roll);
    }
    
    
    State? DoOddEffect(PlayerSO player, GateSO gate, int roll)
    {
        switch (gate.Color) 
        {
            case GateColor.BLACK:
                playerSelect.OnSelect = (selectedPlayer) => {
                    gameManager.PlayerAttacksPlayer(player, selectedPlayer, 4 * player.doubleGateAbil);
                    gameManager.NextTurn();
                };
                return State.ChoosingPlayer;

            case GateColor.GREEN:
                gameManager.PlayerChangeHealth(player, roll * player.doubleGateAbil);
                break;

            case GateColor.RED: //player will not be able to attack the red gate first turn
                if(Globals.battleData.turnCount == 1){
                    return 0;
                } else {
                player.doubleDamageToCenter = 2; 
                gameManager.PlayerAttacksCenterGate(player, roll * player.doubleDamageToCenter * player.doubleGateAbil);
                }
                break;

            case GateColor.BLUE:
                player.hasStockade = true;
                player.totalStockade++;
                break;
        }

        return null;
    }

    State? DoEvenEffect(PlayerSO player, GateSO gate, int roll)
    {
        switch (gate.Color) 
        {
            case GateColor.BLACK:
                gameManager.PlayerChangeHealth(player, -4 * player.doubleDamageToSelf * player.doubleGateAbil);
                break;

            case GateColor.GREEN:
                gameManager.HealCenterGate(3 * player.doubleGateAbil);
                break;

            case GateColor.RED: //if the turnCount is == this will not allow the player to attack the red gate first turn chagnes can be made such as even or odd turns
                if(Globals.battleData.turnCount == 1){
                    return 0;
                }
                 else {
                player.totalDamageToGates = 1; //have to figure out how to change from 1 to 0.5
                gameManager.PlayerChangeHealth(player, -roll * player.totalDamageToGates * player.doubleGateAbil);
                }
                break;

            case GateColor.BLUE:
                // random opponent (without a shield) gets shield
                List<PlayerSO> candidates = new();
                foreach (PlayerSO p in playerListSO.list)
                {
                    if (p.isAlive && p != player && !p.hasStockade)
                        candidates.Add(p);
                }

                if (candidates.Count == 0) break;
                
                int randIndex = Random.Range(0, candidates.Count);
                candidates[randIndex].hasStockade = true;
                candidates[randIndex].totalStockade++;
                break;
        }

        return null;
    }

}
