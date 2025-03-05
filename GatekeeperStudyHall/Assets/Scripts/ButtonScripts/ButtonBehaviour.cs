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
    [SerializeField] Material unselectableMaterial;
    [SerializeField] List<Image> buttonImages;

    CursorPreference cursorPref;

    public bool isSelectable = true;
    bool wasSelectable = true; // holds value of isSelectable from last frame
    bool hovering = false;
    bool pressing = false;

    void OnDisable()
    {
        pressing = false;
        hovering = false;
        UpdateMaterials();
    }

    void Start()
    {
        cursorPref = GetComponent<CursorPreference>();
        UpdateMaterials();
    }

    void Update()
    {
        if (isSelectable != wasSelectable)
        {
            wasSelectable = isSelectable;
            cursorPref.type = isSelectable ? CursorType.Pointer : CursorType.Arrow;
            hovering = false;
            pressing = false;
            UpdateMaterials();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => StartHover();
    public void OnPointerExit(PointerEventData eventData) => EndHover();
    public void OnPointerDown(PointerEventData eventData) => StartPress();
    public void OnPointerUp(PointerEventData eventData) => EndPress();


    void StartHover()
    {
        if (!isSelectable) return;
        
        hovering = true;
        UpdateMaterials();
    }

    void EndHover()
    {
        if (!isSelectable) return;
        
        hovering = false;
        UpdateMaterials();
    }

    void StartPress()
    {
        if (!isSelectable) return;
        
        pressing = true;
        UpdateMaterials();
    }

    void EndPress()
    {
        if (!isSelectable) return;
        
        bool finishedClick = hovering && pressing;
        pressing = false;
        UpdateMaterials();
        if (finishedClick) OnClick.Invoke();
    }

    void UpdateMaterials()
    {
        Material mat = defaultMaterial;
        if (!isSelectable) 
            mat = unselectableMaterial;
        else if (hovering)
            mat = pressing ? pressMaterial : hoverMaterial;
        
        foreach (Image image in buttonImages) 
            image.material = mat;
    }
}
