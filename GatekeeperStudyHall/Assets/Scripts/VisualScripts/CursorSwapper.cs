using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CursorType
{
    Arrow = 0, 
    Pointer = 1, 
    TextBeam = 2,
}

public class CursorSwapper : MonoBehaviour
{
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    
    [Space]
    [SerializeField] private Texture2D arrowTexture;
    [SerializeField] private Vector2 arrowOffset = Vector2.zero;
    
    [Space]
    [SerializeField] private Texture2D pointerTexture;
    [SerializeField] private Vector2 pointerOffset = Vector2.zero;
    
    [Space]
    [SerializeField] private Texture2D textBeamTexture;
    [SerializeField] private Vector2 textBeamOffset = Vector2.zero;
    
    CursorType currType = CursorType.Arrow;
    bool isFirstAwake = true;

    [HideInInspector] public CursorSwapper instance; 
    
    void Awake()
    {
        if (!isFirstAwake) return;
        
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        SwapCursor(CursorType.Arrow);
        isFirstAwake = false;
    }

    void Update()
    {
        var type = GetHoveredCursorType();
        
        if (currType != type) 
            SwapCursor(type);
    }
    
    CursorType GetHoveredCursorType()
    {
        // get all event system raycast results of current mouse or touch position
        PointerEventData eventData = new(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        //print(raycastResults.Aggregate("", (current, curRaycastResult) => current + (curRaycastResult.gameObject.name + ", "))));

        // when not hovering over anything, default to the normal arrow
        if (raycastResults.Count == 0) 
            return CursorType.Arrow;

        // consider only the first object that was hit
        GameObject currObject = raycastResults[0].gameObject;
        CursorPreference pref = null;

        // find the first ancestor that has a CursorPreference attached to it
        do {
            pref = currObject.GetComponent<CursorPreference>();
            if (pref != null) break;
            currObject = currObject.transform.parent?.gameObject;
        }
        while (currObject != null);
        
        // if a CursorPreference was found, return its type (otherwise default to arrow)
        return pref == null ? CursorType.Arrow : pref.type;
    }

    void SwapCursor(CursorType type)
    {
        currType = type;
        
        var texture = type switch
        {
            CursorType.Arrow => arrowTexture,
            CursorType.Pointer => pointerTexture,
            CursorType.TextBeam => textBeamTexture,
            _ => arrowTexture
        };
        
        var offset = type switch
        {
            CursorType.Arrow => arrowOffset,
            CursorType.Pointer => pointerOffset,
            CursorType.TextBeam => textBeamOffset,
            _ => arrowOffset
        };
        
        Cursor.SetCursor(texture, offset, cursorMode);
    }
}
