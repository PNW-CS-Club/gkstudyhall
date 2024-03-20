using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueGateScript : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnblueGateClick()
    {
        
    }
}
