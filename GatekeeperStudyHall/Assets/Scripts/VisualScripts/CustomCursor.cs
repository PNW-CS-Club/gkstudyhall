using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTextureDefault;
    [SerializeField] private Vector2 clickOffset = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    
    void Start()
    {
        Cursor.SetCursor(cursorTextureDefault, clickOffset, cursorMode);
    }
}
