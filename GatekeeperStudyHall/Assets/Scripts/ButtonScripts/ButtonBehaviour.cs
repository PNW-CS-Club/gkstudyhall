using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent OnClick;
    
    bool hovering = false;

    void OnDisable()
    {
        if (hovering) EndHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EndHover();
    }


    void StartHover()
    {
        hovering = true;
        //print("StartHover");
    }

    void EndHover()
    {
        hovering = false;
        //print("EndHover");
    }
}
