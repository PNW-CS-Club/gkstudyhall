using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text textbox;
    [SerializeField] GateSO gate;

    void Update()
    {
        textbox.text = gate.Health.ToString();
    }
}
