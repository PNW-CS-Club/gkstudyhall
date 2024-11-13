using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMagnifier : MonoBehaviour, IPointerClickHandler
{
    CardDisplay magnifiedDisplay;

    bool isInitialized = false;

    void Initialize() {
        magnifiedDisplay = transform.GetChild(0).GetComponent<CardDisplay>();
        magnifiedDisplay.transform.localScale = Vector3.one * 2.0f;
    }

    public void Show(CardSO cardData) {
        gameObject.SetActive(true);
        if (!isInitialized) {
            Initialize();
            isInitialized = true;
        }

        magnifiedDisplay.ChangeCardData(cardData);
    }

    public void OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
    }
}
