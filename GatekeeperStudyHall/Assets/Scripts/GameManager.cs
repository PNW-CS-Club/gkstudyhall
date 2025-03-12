using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages health, damage, and turn flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    public State currentState = State.TraitRoll;
    
    [SerializeField] TraitHandler traitHandler;
    [SerializeField] DiceRoll diceRoll;
    [SerializeField] GateBreak gateBreak;

    [SerializeField] PlayerListSO playerListSO;
    public PlayerListSO iPlayerListSO;
    [SerializeField] CardQueue cardQueue;

    [SerializeField] PlayerSelection playerSelect;
    
    [SerializeField] CenterGateSO centerGate;

    bool wasPlayerSelectShowing = false;
    

    void RollEventHandler(object sender, int roll) => UseRollResult(roll);
    void OnEnable() => diceRoll.DoneRollingEvent += RollEventHandler;
    void OnDestroy() => diceRoll.DoneRollingEvent -= RollEventHandler;

    void Awake() 
    {
        // set the amount of players alive to the initial size of the player list
        iPlayerListSO = playerListSO;
        Globals.playersAlive = playerListSO.list.Count;
    }
    
    void Update()
    {
        // show/hide playerSelect only when current state changes to/from ChoosingPlayerState
        bool isPlayerSelectShowing = currentState == State.ChoosingPlayer;
        if (wasPlayerSelectShowing != isPlayerSelectShowing)
        {
            if (isPlayerSelectShowing)
                playerSelect.Show();
            else 
                playerSelect.Hide();
            
            wasPlayerSelectShowing = isPlayerSelectShowing;
        }
    }
    

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
        centerGate.TakeDamageFromPlayer( damage, attacker );
        if (centerGate.Health == 0)
        {
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
        else {
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
    /// <returns><c>true</c> only if the gate's health has reached zero.</returns>
    public void GateChangeHealth(PlayerSO player, GateSO gate, int amount) 
    {
        if (amount < 0)
        {
            gate.TakeDamage(-amount);
            player.totalDamageToGates += -amount;
        }
        else
        {
            gate.Heal(amount);
        }
    }


    /// <summary>
    /// Performs the action expected to happen after the dice is done rolling, depending on the state.
    /// </summary>
    /// <param name="roll">The rolled value of the dice.</param>
    private void UseRollResult(int roll) 
    {
        PlayerSO currentPlayer = playerListSO.list[0];
        if (currentState == State.TraitRoll) 
        {
            if (roll <= 4) 
            {
                State nextState = traitHandler.ActivateCurrentPlayerTrait(roll);
                if (!playerListSO.list[0].isAlive){
                    NextTurn(); // If the player dies from their trait, end their turn
                } else {
                    currentState = nextState; // TODO: what if the player dies after choosing another player?
                }
            }
            else if (roll == 5) 
            {
                // Player rolls a 5, initiate battle with another player
                currentPlayer.battlesStarted++;
                currentState = State.ChoosingPlayer;
                playerSelect.OnSelect = defender => DoBattle(playerListSO.list[0], defender);
            }
            else 
            {
                // Skip turn
                NextTurn();
            }
        }
        else if (currentState == State.AttackingGate) 
        {
            int attack = roll + currentPlayer.increaseGateDamage - currentPlayer.reduceGateDamage;
            attack = Mathf.Max(0, attack); // set to 0 if attack comes out negative
            Debug.Log($"attacking for {attack} damage");
            GateChangeHealth(currentPlayer, Globals.selectedGate, -attack);
            if (Globals.selectedGate.Health == 0) {
                Debug.Log("You broke the gate!");
                currentState = State.BreakingGate;
            }
            else {
                NextTurn();
            }
        }
        else if (currentState == State.BreakingGate) 
        {
            State? maybeNextState = gateBreak.DoBreakEffect(playerListSO.list[0], Globals.selectedGate, roll);
            Globals.selectedGate.Reset();

            if (maybeNextState is State nextState)
                currentState = nextState;
            else
                NextTurn();
        }
        else if (currentState == State.Battling) {
            if (Globals.battleData.isAttackerRolling) {
                // attacker has rolled
                Globals.battleData.attackerRoll = roll;
                Globals.battleData.isAttackerRolling = false;
                Debug.Log( "ATTACKER rolled a " + roll + ", it is now the DEFENDER's turn" );
            } else {
                // defender has rolled
                Globals.battleData.defenderRoll = roll;
                Debug.Log( "DEFENDER rolled a " + roll );

                if (Globals.battleData.IsTied()) {
                    // rolls were equal
                    Globals.battleData.mult++;
                    Globals.battleData.isAttackerRolling = true;
                    Debug.Log( "These rolls were equal...the stakes rise!  Damage is now " + Globals.battleData.mult + "x!" );
                } else {
                    // a player takes damage
                    int damageDealt = Mathf.Abs(Globals.battleData.attackerRoll - Globals.battleData.defenderRoll) * Globals.battleData.mult;
                    PlayerAttacksPlayer(Globals.battleData.Winner, Globals.battleData.Loser, damageDealt);
                    Debug.Log( $"Battle concluded, {Globals.battleData.Loser} should have taken {damageDealt} damage. Continue with turn..." );

                    Globals.battleData = null;
                    // If the current player died, transition to the next player
                    if (!playerListSO.list[0].isAlive) {
                        NextTurn();
                    }
                    else {
                        currentState = State.ChoosingGate;
                    }
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

        List<PlayerSO>players = playerListSO.list;

        players[0].ResetEffects(); // reset all temporary effects the current player may have
        do {      
            players.Insert(players.Count, players[0]);
            players.RemoveAt(0);
        } 
        while (!players[0].isAlive); // TODO: This will loop infinitely if all players are dead

        cardQueue.RepositionCards();
        
        currentState = State.TraitRoll;
    }

    /// <summary>
    /// Start battling with another player
    /// </summary>
    public void DoBattle(PlayerSO attacker, PlayerSO defender)
    {
        Globals.battleData = new Globals.BattleData(attacker, defender);
        currentState = State.Battling;
        Debug.Log($"Battle begun between {attacker.card.characterName} and {defender.card.characterName}");
    }
}
