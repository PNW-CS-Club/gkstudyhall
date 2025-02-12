using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class Hover_Object : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler

{
    // Start is called before the first frame update
 public GameObject hoverTextBox; // The Text or InputField object to show/hide
    private TMP_Text textComponent;

    void Start()
    {
        textComponent = hoverTextBox.GetComponent<TMP_Text>();
        hoverTextBox.SetActive(false); // Hide the Text box initially
    }

    // Triggered when the mouse enters the object's collider
    public void OnPointerEnter(PointerEventData _)
    {
        hoverTextBox.SetActive(true);
    }

    // Triggered when the mouse exits the object's collider
    public void OnPointerExit(PointerEventData _)
    {
        hoverTextBox.SetActive(false);
    }
}
