using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    bool isRolling = false;

    [SerializeField]
    float maxRotation = 20f;

    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (isRolling) {
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-maxRotation, maxRotation));
            spriteRenderer.sprite = sprites[Random.Range(0, 6)];
        }
    }

    public void MyMouseDown() {
        isRolling = true;
    }

    public void MyMouseUp() {
        isRolling = false;
    }
}
