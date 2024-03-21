using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    // if these are different, it means that the checkbox was toggled last frame
    public bool collapsed;
    bool wasCollapsed;

    const float COLLAPSE_HEIGHT_DIFF = 172f;

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

            transform.GetChild(1).GetComponent<Image>().sprite = cardData.art;

            for (int i = 0; i < 4; i++) {
                transform.GetChild(2).GetChild(i + 4).GetComponent<TMPro.TMP_Text>().text = cardData.traitNames[i];
                transform.GetChild(2).GetChild(i + 8).GetComponent<TMPro.TMP_Text>().text = cardData.traitDescriptions[i];
            }

            transform.GetComponent<Image>().color = cardData.outerColor;
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
