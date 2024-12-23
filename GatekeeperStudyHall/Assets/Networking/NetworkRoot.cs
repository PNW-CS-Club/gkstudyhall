using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRoot : MonoBehaviour
{
    public GameObject netPlayerParent;
    public NetworkLogic netLogic;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
