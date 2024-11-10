using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    /// <summary>
    /// When a player is selected, this function is called with the chosen player as the argument.
    /// </summary>
    public Action<PlayerSO> OnSelect;

    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] StateMachine stateMachine;
    [SerializeField, Min(0)] float margin;
    [SerializeField] Vector2 padding;

    bool isInitialized = false;
    List<PlayerSO> playerList;
    RectTransform panel;
    List<GameObject> displayObjects;
    Vector2 cardSize;


    void Initialize()
    {
        playerList = playerListSO.list;
        panel = transform.GetChild(0).GetComponent<RectTransform>();
        displayObjects = new();

        for (int i = 1; i < playerList.Count; i++) {
            CreateDisplayObject();
        }

        cardSize = cardDisplayPrefab.GetComponent<RectTransform>().sizeDelta;
    }


    /// <summary> 
    /// Use this method to enable the player selection panel 
    /// and update its displays with the correct cards. 
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);

        if (!isInitialized) {
            Initialize(); 
            isInitialized = true;
        }

        // add or remove displays until we have the right amount
        while (displayObjects.Count < playerList.Count - 1)
            CreateDisplayObject();
        while (displayObjects.Count > playerList.Count - 1)
            DestroyDisplayObject();

        for (int i = 0; i < displayObjects.Count; i++)
        {
            CardDisplay cardDisplay = displayObjects[i].GetComponent<CardDisplay>();
            cardDisplay.player = playerList[i + 1];
            cardDisplay.ChangeCardData(playerList[i + 1].card);
        }

        Reposition();
    }



    public void Hide()
    {
        gameObject.SetActive(false);
    }



    void CreateDisplayObject()
    {
        GameObject cardObject = Instantiate(cardDisplayPrefab, panel);
        displayObjects.Add(cardObject);

        cardObject.GetComponent<CardDisplay>().type = CardDisplayType.PLAYER_SELECT_OPTION;
    }


    void DestroyDisplayObject()
    {
        GameObject obj = displayObjects[displayObjects.Count - 1];
        displayObjects.RemoveAt(displayObjects.Count - 1);
        Destroy(obj);
    }


    /// <summary>
    /// Corrects the positions of the cards in the selection panel. 
    /// Helpful for when the number of cards to choose from changes.
    /// </summary>
    void Reposition()
    {
        float xOffset = padding.x;
        foreach (GameObject obj in displayObjects)
        {
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 0.5f);
            rect.anchorMax = new Vector2(0f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = new Vector2(xOffset, 0);
            xOffset += cardSize.x + margin;
        }

        panel.sizeDelta = new Vector2(xOffset - margin + padding.x, padding.y * 2 + cardSize.y);
    }
}
