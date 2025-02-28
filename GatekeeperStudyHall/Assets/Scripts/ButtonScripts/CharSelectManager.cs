using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    [SerializeField] PlayerListSO backupListObject;
    List<PlayerSO> playerList;
    List<PlayerSO> backupList;
    public CardSO randomCard;
    public CardSO clearCard;

    [SerializeField] List<PlayerSO> all4Players;
    [SerializeField] List<GameObject> all4CardDisplays;
    
    [SerializeField] List<CardSO> cardList;

    public CardSO selectedCard;
    
    const int MAX_PLAYERS = 4;
    
    private void Start() 
    {
        UnityEngine.Assertions.Assert.IsTrue(all4Players.Count == MAX_PLAYERS, 
            $"all4Players must have {MAX_PLAYERS} elements");
        UnityEngine.Assertions.Assert.IsTrue(all4CardDisplays.Count == MAX_PLAYERS, 
            $"all4CardDisplays must have {MAX_PLAYERS} elements");
        
        Globals.playerList = playerListObject.list;
        playerList = playerListObject.list;
        backupList = backupListObject.list;
        
        playerList.Clear();
        
        foreach (var obj in all4CardDisplays)
            obj.SetActive(false);
        
        Debug.Log($"Globals.sessionMatchesPlayed: {Globals.sessionMatchesPlayed}");
        if (Globals.sessionMatchesPlayed == 0) {
            // On the first match of the session, we just add 2 new players
            AddPlayer(clearCard);
            AddPlayer(clearCard);
        }
        else {
            // If it's not the first match then we add the previously selected players
            foreach (var backupPlayer in backupList)
                AddPlayer(backupPlayer.card);
        }
    }

    /// Cleans up and starts the match if everyone has selected their character
    public void StartGame()
    {
        // return early if anyone hasn't selected their character yet
        foreach (var player in playerList)
        {
            if (player.card == clearCard) return;
        }
        
        backupList.Clear();

        // initialize each player and copy them to the backup list
        foreach (var player in playerList)
        {
            // if the player chose the random selection, back their card up *before* it is replaced
            // note: nevermind this doesn't work because the backup list copies the players instead of the cards
            backupList.Add(player);
            
            if (player.card == randomCard) {
                int r = Random.Range(0, cardList.Count);
                player.card = cardList[r];
            }
            
            player.GameReset();
            player.ResetEffects();
        }
        
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
    }
    
    /// Return to the title menu
    public void StartMenu()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("StartScene");
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
