using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharSelectManager : MonoBehaviour
{
    // [SerializeField] PlayerListSO playerListObject;
    // List<PlayerSO> playerList; // refers to list in playerListObject
    
    // [SerializeField] PlayerSO player1;

    // // Awake is called as soon as the object is created
    // private void Start() 
    // {
    //     playerList = playerListObject.list;
    //     playerList.Clear();
    //     playerList.Add(player1);
    // }


    private GameObject selectedCharacter = null; // Personaggio selezionato
    private GameObject selectedPlayerSquare = null; // Riquadro del giocatore selezionato

    public List<GameObject> playerSquares; // Lista dei 4 riquadri dei giocatori
    public GameObject placeholderCharacterPrefab; // Prefab del personaggio da instanziare

    // Metodo chiamato al clic sul riquadro del giocatore
    public void OnPlayerSquareClick(GameObject playerSquare)
    {
        if (selectedCharacter != null)
        {
            AssignCharacterToSquare(playerSquare); // Assegna il personaggio selezionato al riquadro
            selectedCharacter = null; // Resetta la selezione del personaggio
        }
        else
        {
            selectedPlayerSquare = playerSquare; // Seleziona il riquadro del giocatore
        }
    }

    // Metodo chiamato al clic sul personaggio
    public void OnCharacterClick(GameObject character)
    {
        selectedCharacter = character; // Seleziona il personaggio
    }

    // Metodo per assegnare il personaggio al riquadro
    private void AssignCharacterToSquare(GameObject playerSquare)
    {
        if (selectedCharacter != null)
        {
            GameObject characterInstance = Instantiate(placeholderCharacterPrefab, playerSquare.transform.position, Quaternion.identity);
            characterInstance.transform.SetParent(playerSquare.transform); // Associa il personaggio al riquadro
        }
    }
}
