using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OptionsTabPress : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnityEvent OnClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }
}
