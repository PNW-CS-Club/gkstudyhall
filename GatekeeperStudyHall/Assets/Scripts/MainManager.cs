using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    
    public static MainManager Instance; // Create a static instance of the MainManager Class. This will allow for data persistence between scenes
    // Note: static means that the values stored in this class will be shared by all the instances of this class

    [SerializeField] PlayerListSO playerListObject;
    public List<PlayerSO> PlayerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;


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

        PlayerList = playerListObject.list;
        PlayerList.Clear();
        PlayerList.Add(player1);
    }

}
