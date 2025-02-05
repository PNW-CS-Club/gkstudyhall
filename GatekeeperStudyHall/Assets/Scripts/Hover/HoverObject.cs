using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // For accessing UI components

public class HoverWithPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverTextBox; // The Text or InputField object to show/hide
    private Text textComponent;

    void Start()
    {
        textComponent = hoverTextBox.GetComponent<Text>();
        hoverTextBox.SetActive(false); // Hide the Text box initially
    }

    // Triggered when the mouse enters the object's collider
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTextBox.SetActive(true);
        textComponent.text = "";
    }

    // Triggered when the mouse exits the object's collider
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTextBox.SetActive(false);
    }
}

