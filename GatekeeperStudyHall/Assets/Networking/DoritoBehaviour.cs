using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoritoBehaviour : NetworkBehaviour
{
    [SerializeField] NetworkVariable<Color> color = new(
        Color.green, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);
    
    SpriteRenderer rend;
    
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        rend.color = color.Value;
        if (!IsOwner) return;

        var rot = transform.rotation.eulerAngles;
        rot.z += Time.deltaTime * 10f;
        transform.rotation = Quaternion.Euler(rot);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeColor();
        }
        
        float dy = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            dy++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dy--;
        }
        
        transform.position += new Vector3(0, dy * Time.deltaTime, 0);
    }

    private void RandomizeColor()
    {
        color.Value = Random.ColorHSV(0.04f, 0.1f, 0.7f, 1, 0.7f, 1);
    }
}
