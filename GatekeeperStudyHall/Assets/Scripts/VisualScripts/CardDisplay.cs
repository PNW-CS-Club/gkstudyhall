using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;


public enum CardDisplayType
{
    [InspectorName("None")]                     NONE = 0,
    [InspectorName("Player Slot")]              PLAYER_SLOT = 1,
    [InspectorName("Character Select Option")]  CHAR_SELECT_OPTION = 2,
    [InspectorName("Player Select Option")]     PLAYER_SELECT_OPTION = 3,
}

[ExecuteInEditMode]
public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlayerSO player; // the player that this card display belongs to
    public CardSO cardData;

    [SerializeField] bool startExpanded = true;
    Vector2 expandedSize;
    Vector2 collapsedSize;
    RectTransform rectTransform;

    public CardDisplayType type = CardDisplayType.NONE;

    [Header("Magnifying")]
    public CardMagnifier cardMagnifier;
    public bool canMagnify = true;

    public const float EXPANDED_HEIGHT = 350f;
    public const float COLLAPSED_HEIGHT = 178f;
    const float HIGHLIGHT_STRENGTH = 0.20f; // 0 -> no highlight; 1 -> full white

    CharSelectManager charSelectManager;
    PlayerSelection playerSelection;


    // enter and exit functions turn the highlight on and off
    public void OnPointerEnter(PointerEventData eventData) {
        if (cardData != null && canMagnify) {
            transform.GetComponent<Image>().color = Color.Lerp(cardData.innerColor, Color.white, HIGHLIGHT_STRENGTH);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (cardData != null) {
            transform.GetComponent<Image>().color = cardData.innerColor;
        }
    }


    public void OnPointerClick(PointerEventData eventData) {
        if (cardData == null) 
            return;

        if (eventData.button == PointerEventData.InputButton.Left) {
            // select or assign card data on left click
            switch (type)
            {
                case CardDisplayType.PLAYER_SLOT:
                    if (charSelectManager.selectedCard != null)
                    {
                        cardData = charSelectManager.selectedCard;
                        charSelectManager.selectedCard = null;
                        player.card = cardData;
                        UpdateDisplay();
                    }
                    break;

                case CardDisplayType.CHAR_SELECT_OPTION:
                    charSelectManager.selectedCard = cardData;
                    break;

                case CardDisplayType.PLAYER_SELECT_OPTION:
                    Debug.Log("Card selected: " + cardData.characterName);
                    Assert.IsNotNull(playerSelection.OnSelect, "The OnSelect callback must have a value at this point");

                    playerSelection.OnSelect(player);
                    playerSelection.OnSelect = null;
                    break;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            // magnify on right click
            if (canMagnify) {
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
        transform.GetChild(2).gameObject.SetActive(isExpanded);
        rectTransform.sizeDelta = (isExpanded ? expandedSize : collapsedSize);
    }



    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        expandedSize = new Vector2(rectTransform.rect.size.x, EXPANDED_HEIGHT);
        collapsedSize = new Vector2(rectTransform.rect.size.x, COLLAPSED_HEIGHT);

        SetExpanded(startExpanded);
    }


    void Start()
    {
        cardMagnifier = FindAnyObjectByType<CardMagnifier>(FindObjectsInactive.Include);
        if (cardMagnifier == null) {
            Debug.LogError("Could not find a CardMagnifier in scene");
        }

        charSelectManager = FindAnyObjectByType<CharSelectManager>(); 
        playerSelection = FindAnyObjectByType<PlayerSelection>();

        // make sure these are not null when it is necessary
        switch (type)
        {
            case CardDisplayType.PLAYER_SLOT:
            case CardDisplayType.CHAR_SELECT_OPTION:
                Assert.IsNotNull(charSelectManager, $"There must be a CharSelectManager in the scene if the card type is {type}");
                break;
            case CardDisplayType.PLAYER_SELECT_OPTION:
                Assert.IsNotNull(playerSelection, $"There must be a PlayerSelection in the scene if the card type is {type}");
                break;
        }

        // use the card data from the player if there is one
        if (player != null) {
            ChangeCardData(player.card);
        }else{
            UpdateDisplay();
        }
        
    }



    /// set the display's colors, text, and art to the values in the current CardSO
    private void UpdateDisplay()
    {
        if (!cardData) return;
        
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
