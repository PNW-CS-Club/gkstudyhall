using UnityEngine;

int turn = 0;

public enum Ability
{
    loseTwo,
    Ability2,
    Ability3,
    // Add more abilities as needed
}

public class Player
{
    public string playerName;
    public int playerHealth = 100;
    public bool hasShield = false;
    public Ability[] abilities;

    public Player(string name, Ability[] abilities)
    {
        playerName = name;
        this.abilities = abilities;
    }

    public void UseAbility(int abilityIndex)
    {
        if (abilityIndex >= 0 && abilityIndex < abilities.Length)
        {
            switch (abilities[abilityIndex])
            {
                case Ability.loseTwo:
                    loseTwo();
                    break;
                case Ability.Ability2:
                    Ability2();
                    break;
                // Add cases for more abilities

                default:
                    Debug.LogError("Invalid ability!");
                    break;
            }
        }
        else
        {
            Debug.LogError("Invalid ability index!");
        }
    }

    void loseTwo()
    {
        TakeDamage(2);
    }

    void Ability2()
    {
        // Code for ability 2
    }

    // Define more abilities as needed

    public void TakeDamage(int damageAmount)
    {
        if (hasShield)
        {
            hasShield = false;
            Debug.Log("Sheild has been lost, no damage taken");
        }
        else
        {
            playerHealth -= damageAmount;
            Debug.Log("You have lost" + damageAmount + " health");
        }

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(playerName + " has died.");
    }

    int turnSwitch(int turn)
    {
        //Change 4 to variable for amountOfPlayers
        return (turn + 1) % 4;
    }
}

public class PlayerController : MonoBehaviour
{
    public Player[] players;

    void Start()
    {
        // Example initialization of players
        players = new Player[4]; // Assuming 4 players max
        players[0] = new Player("Player 1", new Ability[] { Ability.loseTwo, Ability.Ability2 });
        players[1] = new Player("Player 2", new Ability[] { Ability.loseTwo, Ability.Ability3 });
        // Initialize more players as needed
    }

    // Example function to use ability for a specific player
    public void UseAbilityForPlayer(int playerIndex, int abilityIndex)
    {
        if (playerIndex >= 0 && playerIndex < players.Length)
        {
            players[playerIndex].UseAbility(abilityIndex);
        }
        else
        {
            Debug.LogError("Invalid player index!");
        }
    }
}
