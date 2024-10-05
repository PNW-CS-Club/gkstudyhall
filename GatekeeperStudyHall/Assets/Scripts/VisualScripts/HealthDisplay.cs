using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] PlayerSO player;

    float initialWidth;
    RectTransform slider;
    TMPro.TMP_Text healthCounter;

    void Start() 
    {
        slider = transform.GetChild(0).GetComponent<RectTransform>();
        initialWidth = slider.sizeDelta.x;
        healthCounter = transform.GetComponentInChildren<TMPro.TMP_Text>();
    }

    void Update()
    {
        float healthRatio = Mathf.Clamp(player.health, 0, PlayerSO.MAX_HEALTH) / (float)PlayerSO.MAX_HEALTH;
        slider.sizeDelta = new Vector2(initialWidth * healthRatio, slider.sizeDelta.y);
        healthCounter.text = player.health.ToString();
    }
}
