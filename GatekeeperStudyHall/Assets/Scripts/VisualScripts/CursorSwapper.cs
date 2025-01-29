using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
        {
            SwapCursor((CursorType)(((int)currType + 1) % 3));
        }
    }

    public void SwapCursor(CursorType type)
    {
        if (currType == type) return;
        
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
