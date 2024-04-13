using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMagnifier : MonoBehaviour, IPointerClickHandler
{
    CardDisplay magnifiedDisplay;

    void Start() {
        magnifiedDisplay = transform.GetChild(0).GetComponent<CardDisplay>();
        magnifiedDisplay.transform.localScale = Vector3.one * 2.0f;
        gameObject.SetActive(false);
    }

    public void Show(CardSO cardData) {
        magnifiedDisplay.ChangeCardData(cardData);
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
    }
}
