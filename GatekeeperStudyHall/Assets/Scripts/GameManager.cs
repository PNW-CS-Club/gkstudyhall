using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides methods for actions related to health, damage, and turn flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] StateMachine stateMachine;
    [SerializeField] TraitHandler traitHandler;
    [SerializeField] GateBreak gateBreak;

    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] CardQueue cardQueue;

    [SerializeField] PlayerSelection playerSelect;
    
    [SerializeField] CenterGateSO centerGate;

    [SerializeField] GameObject offlineDicePrefab;
    [SerializeField] NetworkObject networkDicePrefab;
    [SerializeField] Vector3 diceSpawnPoint;
    [SerializeField] GameObject diceBarrier;
    DiceRoll diceRoll;


    void Awake()
    {
        if (Globals.multiplayerType == MultiplayerType.Offline)
            Instantiate(offlineDicePrefab, diceSpawnPoint, Quaternion.identity);
        else
            NetworkRoot.GkAwake(networkDicePrefab, diceSpawnPoint);
    }
    

    void Start()
    {
        var dice = GameObject.FindWithTag("Dice");
        diceRoll = dice.GetComponent<DiceRoll>();
        diceRoll.stateMachine = stateMachine;
        diceRoll.barrier = diceBarrier;
        diceRoll.DoneRollingEvent += RollEventHandler;
        
        playerListSO.list[0].isUp = true;
    }
    
    void RollEventHandler(object sender, int roll) => UseRollResult(roll);

    /// <summary>
    /// Determine whether there is a single player alive.
    /// The last player standing wins the game. 
    /// </summary>
    public void CheckWinBySurvival()
    {
        if (Globals.playersAlive == 1) {
            // Determine the last player alive
            List<PlayerSO> playerList = playerListSO.list;
            foreach(PlayerSO player in playerList){
                if (player.isAlive) {
                    Globals.winningPlayer = player;  
                    break;
                }
            }
            
            // Transition to End Scene
            AsyncOperation _ = SceneManager.LoadSceneAsync("EndScene");

        }
    }

    /// <summary>
    /// Plays out the effects of one player attacking another player.
    /// </summary>
    public void PlayerAttacksPlayer(PlayerSO attacker, PlayerSO defender, int damage)
    {
        defender.TakeDamage(damage);
        attacker.totalDamageToOtherPlayers +=  damage;
        Debug.Log($"{attacker.name} attacked {defender.name} for {damage} damage!");
        CheckWinBySurvival();
    }

   
    /// <summary>
    /// <para>Reduces the health of the center gate.</para>
    /// <para>If the center gate's health is reduced to zero, the game ends.</para>
    /// </summary>
    /// <param name="attacker">The player attacking the center gate</param>
    /// <param name="damage"></param>
    public void PlayerAttacksCenterGate(PlayerSO attacker, int damage)
    {
        centerGate.TakeDamage(damage);
        attacker.totalDamageToGatekeeper += damage;
        if (centerGate.Health == 0)
        {
            //Debug.Log($"{attacker.card.characterName} wins! End the game here");
            Globals.winningPlayer = attacker;
            SceneManager.LoadScene("EndScene");
        }
    }


    public void HealCenterGate(int amount) => centerGate.Heal(amount);


    /// <summary>
    /// Changes the health of a player by the specified amount. 
    /// A player's health is clamped between <c>0</c> and <c>PlayerSO.MAX_HEALTH</c>.
    /// Use this method in the scenario that the player should gain or lose health but is not being attacked.
    /// </summary>
    /// <param name="player">The player whose health changes.</param>
    /// <param name="amount">The amount of health to change by (positive to heal, negative to take damage).</param>
    public void PlayerChangeHealth(PlayerSO player, int amount)
    {
        if (amount < 0)
        {
            player.TakeDamage(-amount);
            CheckWinBySurvival();
        }
            
        else{
            player.Heal(amount);
        }
            
    }


    /// <summary>
    /// Changes the health of a gate by a specified amount.
    /// Use this method whenever a player causes a gate to change health.
    /// </summary>
    /// <param name="player">The player who caused the change.</param>
    /// <param name="gate">The gate that is changing health.</param>
    /// <param name="amount">The amount to change by (positive to heal, negative to deal damage)</param>
    public void GateChangeHealth(PlayerSO player, GateSO gate, int amount) 
    {
        if (amount < 0)
        {
            gate.TakeDamage(-amount);
            player.totalDamageToGates += -amount;
        }
        else 
            gate.Heal(amount);
    }


    /// <summary>
    /// Performs the action expected to happen after the dice is done rolling, depending on the state.
    /// </summary>
    /// <param name="roll">The rolled value of the dice.</param>
    private void UseRollResult(int roll) {
        PlayerSO currentPlayer = playerListSO.list[0];
        if (stateMachine.CurrentState is TraitRollState) 
        {
            if (roll <= 4) 
            {
                IState nextState = traitHandler.ActivateCurrentPlayerTrait(roll);
                if (!playerListSO.list[0].isAlive){
                    NextTurn(); // If the player dies from their trait, end their turn
                } else {
                    stateMachine.TransitionTo(nextState); // TODO: what if the player dies after choosing another player?
                }
            }
            else if (roll == 5) 
            {
                // Player rolls a 5, initiate battle with another player
                currentPlayer.battlesStarted++;
                stateMachine.TransitionTo( stateMachine.choosingPlayerState );
                stateMachine.choosingPlayerState.playerSelect.OnSelect = ( defender ) => {
                    Debug.Log( playerListSO.list[ 0 ] );
                    Debug.Log( defender ); 
                    DoBattle( playerListSO.list[ 0 ], defender );
                };
            }
            else 
            {
                // Skip turn
                NextTurn();
            }
        }
        else if (stateMachine.CurrentState is AttackingGateState) 
        {
            int attack = roll + currentPlayer.increaseGateDamage - currentPlayer.reduceGateDamage;
            attack = Mathf.Max(0, attack); // set to 0 if attack comes out negative
            Debug.Log($"attacking for {attack} damage");
            GateChangeHealth(currentPlayer, Globals.selectedGate, -attack);
            if (Globals.selectedGate.Health == 0) {
                Debug.Log("You broke the gate!");
                stateMachine.TransitionTo(stateMachine.breakingGateState);
            }
            else {
                NextTurn();
            }
        }
        else if (stateMachine.CurrentState is BreakingGateState)
        {
            IState nextState = gateBreak.DoBreakEffect(playerListSO.list[0], Globals.selectedGate, roll);
            Globals.selectedGate.Reset();
            
            if (nextState == null)
                NextTurn();
            else 
                stateMachine.TransitionTo(nextState);
        }
        else if ( stateMachine.CurrentState is BattlingState ) {
            if ( Globals.BattleData.isAttackerRolling ) {
                // attacker has rolled
                Globals.BattleData.data[ 0 ] = new( Globals.BattleData.data[ 0 ].player, roll );
                Globals.BattleData.isAttackerRolling = false;
                Debug.Log( "ATTACKER rolled a " + roll + ", it is now the DEFENDER's turn" );
            } else {
                // defender has rolled
                Globals.BattleData.data[ 1 ] = new( Globals.BattleData.data[ 1 ].player, roll );
                Debug.Log( "DEFENDER rolled a " + roll );

                if ( Globals.BattleData.data[ 0 ].roll == Globals.BattleData.data[ 1 ].roll ) {
                    // rolls were equal
                    Globals.BattleData.mult++;
                    Globals.BattleData.isAttackerRolling = true;
                    Debug.Log( "These rolls were equal...the stakes rise!  Damage is now " + Globals.BattleData.mult + "x!" );
                } else {
                    // a player takes damage
                    bool attackerDealsDamage = Globals.BattleData.data[ 0 ].roll > Globals.BattleData.data[ 1 ].roll;
                    PlayerSO damageDealer = attackerDealsDamage ? Globals.BattleData.data[ 0 ].player : Globals.BattleData.data[ 1 ].player;
                    PlayerSO damageTaker  = attackerDealsDamage ? Globals.BattleData.data[ 1 ].player : Globals.BattleData.data[ 0 ].player;

                    int damageDealt = Mathf.Abs( Globals.BattleData.data[ 0 ].roll - Globals.BattleData.data[ 1 ].roll ) * Globals.BattleData.mult;

                    PlayerAttacksPlayer( damageDealer, damageTaker, damageDealt );
                    
                    Debug.Log( $"Battle concluded, {damageTaker} should have taken {damageDealt} damage. Continue with turn..." );

                    Globals.BattleData.Reset();
                    // If the current player died, transition to the next player
                    if (!playerListSO.list[0].isAlive){
                        NextTurn();
                    }
                    stateMachine.TransitionTo( stateMachine.choosingGateState );
                }
            }
        }
        else 
        {
            Debug.LogError("The player should not be able to roll the dice now!");
        }
    }


    /// <summary>
    /// Rotates the player list so that the next person up is in position zero. 
    /// This makes them the "current player."
    /// Then updates the card queue to visually match the new player list orientation.
    /// </summary>
    public void NextTurn() 
    {
        Globals.selectedGate = null;

        List<PlayerSO> players = playerListSO.list;

        players[0].ResetEffects(); //reset all temporary effects the current player may have
        players[0].isUp = false;
        
        do {      
            players.Insert(players.Count, players[0]);
            players.RemoveAt(0);
        } 
        while (!players[0].isAlive); // TODO: This will loop infinitely if all players are dead

        players[0].isUp = true;
        if (NetworkPlayer.ownedInstance.player.isUp)
        {
            // TODO: gain dice ownership somehow
        }
        
        cardQueue.RepositionCards();

        stateMachine.TransitionTo(stateMachine.traitRollState);
    }

    /// <summary>
    /// Start battling with another player
    /// </summary>
    public void DoBattle( PlayerSO attacker, PlayerSO defender ) {
        Globals.BattleData.data.Add( new( attacker, 0 ) );
        Globals.BattleData.data.Add( new( defender, 0 ) );

        Globals.BattleData.mult = 1;

        stateMachine.TransitionTo( stateMachine.battlingState );

        Debug.Log( "Battle begun with ATTACKER = " + attacker.card.characterName + " vs  DEFENDER = " + defender.card.characterName );

        Globals.BattleData.isAttackerRolling = true;
    }
}
