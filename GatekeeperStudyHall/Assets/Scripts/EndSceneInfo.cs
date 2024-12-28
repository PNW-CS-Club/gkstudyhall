using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{
    [SerializeField] CardDisplay cardDisplay1;
    [SerializeField] CardDisplay cardDisplay2;
    [SerializeField] CardDisplay cardDisplay3;
    [SerializeField] CardDisplay cardDisplay4;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{Globals.winningPlayer} won the game!");
                
    }

    void Awake(){
        cardDisplay1.cardData = Globals.winningPlayer.card;
    }

    // Update is called once per frame
    void Update() {}
}
