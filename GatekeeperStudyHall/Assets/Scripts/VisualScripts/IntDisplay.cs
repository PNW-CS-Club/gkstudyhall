using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textbox;
    [SerializeField] IntReference intReference;

    void Update()
    {
        textbox.text = intReference.value.ToString();
    }
}
