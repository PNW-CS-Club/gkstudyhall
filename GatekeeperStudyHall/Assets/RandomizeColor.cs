using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColor : MonoBehaviour
{
    void Update()
    {
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
