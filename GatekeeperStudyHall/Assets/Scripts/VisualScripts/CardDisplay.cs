using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] PlayerSO player; // the player that this card display belongs to
    public CardSO cardData;

    public CardMagnifier cardMagnifier;
    public bool canMagnify = true;

    public bool isPlayerSlot = false; //if the card selected is a slot

    [SerializeField] bool startExpanded = true;
    Vector2 expandedSize;
    Vector2 collapsedSize;

    public const float COLLAPSE_HEIGHT_DIFF = 172f;
    const float HIGHLIGHT_STRENGTH = 0.20f; // 0 -> no highlight; 1 -> full white

    public static CardSO selectedCardSO; 


    // enter and exit functions turn the highlight on and off
    public void OnPointerEnter(PointerEventData eventData) {
        if (canMagnify && cardData != null) {
            transform.GetComponent<Image>().color = Color.Lerp(cardData.innerColor, Color.white, HIGHLIGHT_STRENGTH);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (cardData != null) {
            transform.GetComponent<Image>().color = cardData.innerColor;
        }
    }

    // update the visualization of the card
    public void SelectCard(CardSO selectedCardData)
    {
        ChangeCardData(selectedCardData);
    }

    //if is select a card the player change card
    private void AssignCardSOToPlayerSlot()
    {
        if (selectedCardSO != null)
        {
            cardData = selectedCardSO;
            Debug.Log("Card assign to " + gameObject.name + ": " + cardData.characterName);
            selectedCardSO = null;
            player.card = cardData; // assign the card to the playerSO
            SelectCard(cardData);
        }
        else
        {
            Debug.LogError("No card assign");
        }
    }


    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            // select on left click
            if(isPlayerSlot && selectedCardSO != null) {
                cardData = selectedCardSO;
                AssignCardSOToPlayerSlot();
            }
            else if (!isPlayerSlot && cardData != null) {
                selectedCardSO = cardData; 
                Debug.Log("Card selected: " + cardData.characterName);
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Right) {
            if (canMagnify && cardData != null) {
                cardMagnifier.Show(cardData);
            }
        }
    }



    // change which card this gameobject is displaying
    public void ChangeCardData(CardSO data) {
        cardData = data;
        UpdateDisplay();
    }


    public void SetExpanded(bool isExpanded)
    {
        Debug.Log("Set isExpanded: " + isExpanded);
        transform.GetChild(2).gameObject.SetActive(isExpanded);
        GetComponent<RectTransform>().sizeDelta = (isExpanded ? expandedSize : collapsedSize);
    }



    void Awake()
    {
        expandedSize = GetComponent<RectTransform>().rect.size;
        collapsedSize = expandedSize - new Vector2(0, COLLAPSE_HEIGHT_DIFF);

        SetExpanded(startExpanded);
    }


    void Start()
    {
        cardMagnifier = (CardMagnifier) FindAnyObjectByType(typeof(CardMagnifier), FindObjectsInactive.Include);
        if (cardMagnifier == null) {
            Debug.LogError("Could not find a CardMagnifier in scene");
        }

        UpdateDisplay();
    }



    // set the display's colors, text, and art to the values in the current CardSO
    private void UpdateDisplay() {
        if (cardData != null) {
            transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = cardData.characterName;

            transform.GetChild(1).GetComponent<Image>().color = cardData.outerColor;
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cardData.art;

            for (int i = 0; i < 4; i++) {
                transform.GetChild(2).GetChild(i + 4).GetComponent<TMPro.TMP_Text>().text = cardData.traitNames[i];
                transform.GetChild(2).GetChild(i + 8).GetComponent<TMPro.TMP_Text>().text = cardData.traitDescriptions[i];
            }

            transform.GetComponent<Image>().color = cardData.innerColor;
        }
    }
}
