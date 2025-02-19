#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageStretch : MonoBehaviour
{
    [SerializeField] Image leftImage;
    [SerializeField] Image middleImage;
    [SerializeField] Image rightImage;

    Vector2 prevSize = Vector2.zero;
    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        // do early returns so the rest of the logic doesn't have to run everytime something updates
        if (rt.rect.size == prevSize)
            return;
        
        if (leftImage == null || rightImage == null || middleImage == null)
            return;

        var updatedImages = new Object[] {leftImage, middleImage, rightImage};
        Undo.RecordObjects(updatedImages, $"Change size of images (id: {GetInstanceID()})");
        PrefabUtility.RecordPrefabInstancePropertyModifications(leftImage);
        PrefabUtility.RecordPrefabInstancePropertyModifications(middleImage);
        PrefabUtility.RecordPrefabInstancePropertyModifications(rightImage);

        Rect leftRect = leftImage.sprite.rect;
        float leftRatio = leftRect.width / leftRect.height;
        leftImage.rectTransform.sizeDelta = new Vector2(leftRatio, 1) * rt.rect.size.y;
        
        Rect rightRect = rightImage.sprite.rect;
        float rightRatio = rightRect.width / rightRect.height;
        rightImage.rectTransform.sizeDelta = new Vector2(rightRatio, 1) * rt.rect.size.y;

        float middleWidth = rt.rect.size.x - leftImage.rectTransform.sizeDelta.x - rightImage.rectTransform.sizeDelta.x;
        middleImage.rectTransform.sizeDelta = new Vector2(middleWidth, rt.rect.size.y);

        prevSize = rt.rect.size;
    }
}
#endif
