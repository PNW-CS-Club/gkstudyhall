using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    public bool highlightOnMouseOver = true;

    // if these are different, it means that the checkbox was toggled last frame
    public bool collapsed;
    bool wasCollapsed;

    const float COLLAPSE_HEIGHT_DIFF = 172f;
    const float HIGHLIGHT_STRENGTH = 0.20f; // 0 -> no highlight; 1 -> full white



    // enter and exit functions turn the highlight on and off
    public void OnPointerEnter(PointerEventData eventData) {
        if (highlightOnMouseOver) {
            transform.GetComponent<Image>().color = Color.Lerp(cardData.innerColor, Color.white, HIGHLIGHT_STRENGTH);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.GetComponent<Image>().color = cardData.innerColor;
    }



    void Start()
    {
        // start expanded by default
        wasCollapsed = collapsed;
        if (collapsed) {
            Collapse();
        }

        UpdateDisplay();
    }


    void Update() {
        if (wasCollapsed != collapsed) {
            wasCollapsed = collapsed;
            if (collapsed) {
                Collapse();
            } else {
                Expand();
            }
        }
    }



    private void UpdateDisplay() {
        if (cardData != null) {
            transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = cardData.name;

            transform.GetChild(1).GetComponent<Image>().color = cardData.outerColor;
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cardData.art;

            for (int i = 0; i < 4; i++) {
                transform.GetChild(2).GetChild(i + 4).GetComponent<TMPro.TMP_Text>().text = cardData.traitNames[i];
                transform.GetChild(2).GetChild(i + 8).GetComponent<TMPro.TMP_Text>().text = cardData.traitDescriptions[i];
            }

            transform.GetComponent<Image>().color = cardData.innerColor;
        }
    }



    private void Collapse() {
        transform.GetChild(2).gameObject.SetActive(false);

        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        rectTransform.sizeDelta = new Vector2(rect.width, rect.height - COLLAPSE_HEIGHT_DIFF);
    }

    private void Expand() {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        rectTransform.sizeDelta = new Vector2(rect.width, rect.height + COLLAPSE_HEIGHT_DIFF);

        transform.GetChild(2).gameObject.SetActive(true);
    }
}
