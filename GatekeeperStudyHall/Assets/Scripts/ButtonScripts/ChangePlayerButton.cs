using UnityEngine;

public class ChangePlayerButton : MonoBehaviour
{
    public CardDisplay cardDisplay; // Riferimento a CardDisplay
    public CardSO cardData; // Dati della carta per il bottone

    public void OnClick()
    {
        if (cardDisplay != null && cardData != null)
        {
            cardDisplay.SelectCard(cardData); // Invoca SelectCard direttamente
        }
    }

}