using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleButtonOnClick : MonoBehaviour, IPointerClickHandler
{
    private Image circleButton;
    // Start is called before the first frame update
    void Start()
    {
        circleButton = GetComponent<Image>();
    }

    
    public void OnPointerClick(PointerEventData eventData){
        //Get position of the click relative to the center of the button
        Vector2 localClickPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(circleButton.rectTransform, eventData.position, eventData.pressEventCamera, out localClickPosition);

        //calculate the distance from the center of the button
        float distance = localClickPosition.magnitude;

        //if the distance is within the radius of the button
        if(distance <= circleButton.rectTransform.sizeDelta.x / 2f){
            Debug.Log(Random.Range(1,7));
        }
    }
}
