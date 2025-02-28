using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnClick;
    
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material hoverMaterial;
    [SerializeField] Material pressMaterial;
    [SerializeField] List<Image> buttonImages;
    
    bool hovering = false;
    bool pressing = false;

    void OnDisable()
    {
        pressing = false;
        hovering = false;
        UpdateMaterials();
    }

    public void OnPointerEnter(PointerEventData eventData) => StartHover();
    public void OnPointerExit(PointerEventData eventData) => EndHover();
    public void OnPointerDown(PointerEventData eventData) => StartPress();
    public void OnPointerUp(PointerEventData eventData) => EndPress();


    void StartHover()
    {
        hovering = true;
        UpdateMaterials();
    }

    void EndHover()
    {
        hovering = false;
        UpdateMaterials();
    }

    void StartPress()
    {
        pressing = true;
        UpdateMaterials();
    }

    void EndPress()
    {
        bool finishedClick = hovering && pressing;
        pressing = false;
        UpdateMaterials();
        if (finishedClick) OnClick.Invoke();
    }

    void UpdateMaterials()
    {
        Material mat = defaultMaterial;
        if (hovering)
            mat = pressing ? pressMaterial : hoverMaterial;
        
        foreach (Image image in buttonImages) 
            image.material = mat;
    }
}
