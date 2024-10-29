using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMagnifier : MonoBehaviour, IPointerClickHandler
{
    CardDisplay magnifiedDisplay;

    bool isInitialized = false;

    void OnEnable() {
        if (!isInitialized) {
            magnifiedDisplay = transform.GetChild(0).GetComponent<CardDisplay>();
            magnifiedDisplay.transform.localScale = Vector3.one * 2.0f;
            
            isInitialized = true;
        }
    }

    public void Show(CardSO cardData) {
        gameObject.SetActive(true);
        magnifiedDisplay.ChangeCardData(cardData);
    }

    public void OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
    }
}
