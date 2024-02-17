using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollButtonAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiceRoll(){
        int rollOutcome = Random.Range(1,7); //Random.Range returns a float which is then truncated to an int so the range will be 1 - 6
        Debug.Log(rollOutcome);
    }
}
