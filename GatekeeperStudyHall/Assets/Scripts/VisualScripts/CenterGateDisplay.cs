using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGateDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textbox;
    [SerializeField] CenterGateSO centerGate;

    void Update()
    {
        textbox.text = centerGate.health.ToString();
    }
}
