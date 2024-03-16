using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    void Start()
    {
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
}
