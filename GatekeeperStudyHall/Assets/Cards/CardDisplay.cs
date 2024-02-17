using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    void Start()
    {
        Debug.Log(cardData.ToString());
    }
}
