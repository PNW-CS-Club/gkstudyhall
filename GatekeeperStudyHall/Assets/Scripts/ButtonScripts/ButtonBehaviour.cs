using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnClick;
    [SerializeField] Gradient gradient;
    
    bool hovering = false;

    void OnDisable()
    {
        if (hovering)
        {
            print("OnDisable");
            hovering = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        print("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        print("OnPointerExit");
    }
}
