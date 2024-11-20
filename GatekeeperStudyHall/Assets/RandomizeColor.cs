using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RandomizeColor : NetworkBehaviour
{
    void Update()
    {
        if (!IsOwner) return;

        var rot = transform.rotation.eulerAngles;
        rot.z += Time.deltaTime * 10f;
        transform.rotation = Quaternion.Euler(rot);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SpriteRenderer>().color = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
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
}
