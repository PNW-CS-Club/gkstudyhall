using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList;
    public CardSO randomCard;
    public CardSO clearCard;

    [SerializeField] List<PlayerSO> all4Players;
    [SerializeField] List<GameObject> all4CardDisplays;
    
    [SerializeField] List<CardSO> randomCardChoices;

    public CardSO selectedCard;
    
    List<CardSO> savedCards;
    
    const int MAX_PLAYERS = 4;
    
    private void Start() 
    {
        UnityEngine.Assertions.Assert.IsTrue(all4Players.Count == MAX_PLAYERS, 
            $"all4Players must have {MAX_PLAYERS} elements");
        UnityEngine.Assertions.Assert.IsTrue(all4CardDisplays.Count == MAX_PLAYERS, 
            $"all4CardDisplays must have {MAX_PLAYERS} elements");
        
        playerList = playerListObject.list;
        savedCards = Globals.charSelectCards;
        
        playerList.Clear();
        
        foreach (var obj in all4CardDisplays)
            obj.SetActive(false);

        if (savedCards.Count == 0)
        {
            // occurs only the first time the char select screen is loaded
            savedCards.Add(clearCard);
            savedCards.Add(clearCard);
        }
        
        LoadSelectedCards();
    }

    /// Cleans up and starts the match if everyone has selected their character
    public void StartGame()
    {
        // return early if anyone hasn't selected their character yet
        if (playerList.Any(player => player.card == clearCard)) return;
        
        SaveSelectedCards();
        
        // initialize each player
        foreach (var player in playerList)
        {
            if (player.card == randomCard) {
                int r = Random.Range(0, randomCardChoices.Count);
                player.card = randomCardChoices[r];
            }
            
            player.GameReset();
            player.ResetEffects();
        }
        
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
    }
    
    /// Return to the title menu
    public void StartMenu()
    {
        SaveSelectedCards();
        AsyncOperation _ = SceneManager.LoadSceneAsync("StartScene");
    }

    void SaveSelectedCards()
    {
        savedCards.Clear();
        
        foreach (var player in playerList)
            savedCards.Add(player.card);
    }

    void LoadSelectedCards()
    {
        foreach (var card in savedCards)
            AddPlayer(card);
    }

    /// This function is for the "Add Player" button.
    /// Adds the next player to the list of players.
    public void AddPlayer()
    {
        AddPlayer(clearCard);
    }
    
    /// Overload of AddPlayer that adds the next player and gives them a specific card.
    void AddPlayer(CardSO card)
    {
        var playerIndex = playerList.Count;
        if (playerIndex >= MAX_PLAYERS) return;
        
        all4Players[playerIndex].card = card;
        playerList.Add(all4Players[playerIndex]);
        all4CardDisplays[playerIndex].GetComponent<CardDisplay>().ChangeCardData(card);
        all4CardDisplays[playerIndex].SetActive(true);
    }

    /// This function will be for the AddBotButton.
    /// Makes an instance of the Bot class and adds it to the list of players.
    public void AddBot() { }

    /// This function is for the RemovePlayerButton on both Bots and Players.
    /// Removes the Player/Bot from the list of players and makes the card available again.
    public void RemovePlayer()
    {
        var playerIndex = playerList.Count - 1;
        // there must always be at least 2 players, so we do not remove at [0] or [1]
        if (playerIndex < 2 || playerIndex >= MAX_PLAYERS) return;
        
        playerList.Remove(all4Players[playerIndex]);
        all4CardDisplays[playerIndex].SetActive(false);
    }
}
