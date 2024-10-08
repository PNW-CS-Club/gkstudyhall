using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] PlayerSO player;
    [SerializeField] Color shieldColor;

    Color defaultColor;
    float initialWidth;
    RectTransform slider;
    TMPro.TMP_Text healthCounter;

    bool hadStockadeLastUpdate = false;

    void Start() 
    {
        slider = transform.GetChild(1).GetComponent<RectTransform>();
        defaultColor = slider.GetComponent<Image>().color;
        initialWidth = slider.sizeDelta.x;

        healthCounter = transform.GetComponentInChildren<TMPro.TMP_Text>();

        transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
    }

    void Update()
    {
        float healthRatio = Mathf.Clamp(player.health, 0, PlayerSO.MAX_HEALTH) / (float)PlayerSO.MAX_HEALTH;
        slider.sizeDelta = new Vector2(initialWidth * healthRatio, slider.sizeDelta.y);
        healthCounter.text = player.health.ToString();

        if (player.hasStockade != hadStockadeLastUpdate) 
        {
            if (player.hasStockade) 
            {
                slider.GetComponent<Image>().color = shieldColor;
                // turns on the shield and off the circle
                transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            else 
            {
                slider.GetComponent<Image>().color = defaultColor;
                // turns on the circle and off the shield
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            }

            hadStockadeLastUpdate = player.hasStockade;
        }
    }
}
