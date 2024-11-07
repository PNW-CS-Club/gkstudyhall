using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField, Min(0)] float margin;
    [SerializeField] Vector2 padding;

    bool isInitialized = false;
    List<PlayerSO> playerList;
    RectTransform panel;
    List<GameObject> displayObjects;
    Vector2 cardSize;


    void OnEnable()
    {
        if (!isInitialized)
        {
            playerList = playerListSO.list;
            panel = transform.GetChild(0).GetComponent<RectTransform>();
            displayObjects = new();

            for (int i = 0; i < playerList.Count; i++)
            {
                GameObject cardDisplay = Instantiate(cardDisplayPrefab, panel);
                cardDisplay.GetComponent<CardDisplay>().isSelectable = true;
                displayObjects.Add(cardDisplay);
            }

            cardSize = cardDisplayPrefab.GetComponent<RectTransform>().sizeDelta;

            isInitialized = true;
        }
    }


    public void ShowExcluding(int excludedCardIndex)
    {
        gameObject.SetActive(true);
        displayObjects[playerList.Count - 1].gameObject.SetActive(false);

        int playerIndex = 0;
        int displayIndex = 0;

        while (playerIndex < playerList.Count)
        {
            if (playerIndex != excludedCardIndex)
            {
                displayObjects[displayIndex].GetComponent<CardDisplay>().ChangeCardData(playerList[playerIndex].card);
                displayIndex++;
            }
            playerIndex++;
        }

        Reposition(playerList.Count - 1);
    }


    public void ShowAll()
    {
        gameObject.SetActive(true);
        displayObjects[playerList.Count - 1].SetActive(true);

        for (int i = 0; i < playerList.Count; i++)
        {
            displayObjects[i].GetComponent<CardDisplay>().ChangeCardData(playerList[i].card);
        }

        Reposition(playerList.Count);
    }


    void Reposition(int numDisplays)
    {
        float xOffset = padding.x;
        for (int i = 0; i < numDisplays; i++)
        {
            var rect = displayObjects[i].GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 0.5f);
            rect.anchorMax = new Vector2(0f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = new Vector2(xOffset, 0);
            xOffset += cardSize.x + margin;
        }

        panel.sizeDelta = new Vector2(xOffset - margin + padding.x, padding.y * 2 + cardSize.y);
    }
}
