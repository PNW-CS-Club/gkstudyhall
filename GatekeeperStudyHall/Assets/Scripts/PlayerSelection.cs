using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class PlayerSelection : MonoBehaviour
{
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


    void OnEnable()
    {
        if (!isInitialized)
        {
            playerList = playerListSO.list;
            panel = transform.GetChild(0).GetComponent<RectTransform>();
            displayObjects = new();

            for (int i = 1; i < playerList.Count; i++)
            {
                GameObject cardObject = Instantiate(cardDisplayPrefab, panel);
                CardDisplay display = cardObject.GetComponent<CardDisplay>();

                display.isSelectable = true;
                display.OnSelect.AddListener(TransitionToScheduled);

                displayObjects.Add(cardObject);
            }

            cardSize = cardDisplayPrefab.GetComponent<RectTransform>().sizeDelta;

            isInitialized = true;
        }
    }


    void TransitionToScheduled()
    {
        Assert.IsNotNull(Globals.scheduledState, "The scheduled state must have a non-null value at this point.");

        IState state = Globals.scheduledState;
        Globals.scheduledState = null;
        stateMachine.TransitionTo(state);
    }


    public void Show()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < displayObjects.Count; i++)
        {
            displayObjects[i].GetComponent<CardDisplay>().ChangeCardData(playerList[i+1].card);
        }

        Reposition();
    }


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
