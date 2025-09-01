using System.Collections.Generic;
using UnityEngine;

public class ScreenWipe : MonoBehaviour
{
    [SerializeField] Vector2 displacement = new(-10f, 0f);
    Vector2 startPos;
    
    [SerializeField] float duration = 1f;
    float timer = 0f;
    
    bool doWipe = false;
    
    // Update is called once per frame
    void Start() {
        startPos = transform.position;
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) StartWipe();
        if (doWipe) {
            timer += Time.deltaTime;

            if (timer >= duration) {
                timer = duration;
                doWipe = false;
            }
            
            transform.position = startPos + displacement * (timer / duration);
        }
    }

    public void StartWipe() {
        doWipe = true;
    }
}
