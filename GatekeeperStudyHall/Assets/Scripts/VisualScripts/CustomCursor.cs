using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTextureDefault;
    [SerializeField] private Vector2 clickPosition = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
    }
}
