using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class MainManager : MonoBehaviour
{
    
    public static MainManager Instance; // Create a static instance of the MainManager Class. This will allow for data persistence between scenes
    // Note: static means that the values stored in this class will be shared by all the instances of this class

    // Variables we want to transfer between scenes
    public int Turn = 0; // The current turn is global because we may have a way to choose turn order in a menu prior to starting the game
    public int NumPlayers = 0;
    public List<PlayerInfo> PlayerList = new List<PlayerInfo>();


    // Awake() is called as soon as the object is created
    private void Awake(){
        // This will allow us to access the MainManager object from any other script

        if(Instance != null){
            // We only want a single instance of MainManager at all times
            Destroy(gameObject);
            return;
        }

        Instance = this; // We can now call MainManager.Instance from any other script
        DontDestroyOnLoad(gameObject); // The MainManager GameObject attached to this script will not be destroyed when the scene changes
    }


    //Handle turn switching
    void turnSwitch()
    {   
        /*
        // If a player is dead, their turn will be skipped.
        do{
            Turn = (Turn + 1) % NumPlayers;
        } while (PlayerList[Turn].isdead);
        */
    }

}
