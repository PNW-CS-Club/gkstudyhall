using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    /*
        Stores the following information:

        Player Card
        Health - Initialize to 20 HP
        Stockade - 0 (Player takes no damage when stockade > 1)

        Maybe TurnOrder / Player Number (Could be handled elsewhere)
    */

    private int health = 20;
    private int stockade = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int getHealth(){
        return health;
    }

    int getStockade(){
        return stockade;
    }
}
