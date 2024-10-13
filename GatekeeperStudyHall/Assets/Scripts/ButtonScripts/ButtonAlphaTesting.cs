using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAlphaTesting : MonoBehaviour
{
    void Start()
    {
        // Buttons with this script will only register as "hit" when the alpha of the image at the mouse position is at least 10%
        // The image's "Mesh Type" needs to be set to "Full Rect" and "Read/Write" needs to be toggled on for this to work
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
