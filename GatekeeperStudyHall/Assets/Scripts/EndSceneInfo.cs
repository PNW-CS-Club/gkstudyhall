using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Globals.sessionMatchesPlayed++;

        Debug.Log($"{Globals.winningPlayer} won the game!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
