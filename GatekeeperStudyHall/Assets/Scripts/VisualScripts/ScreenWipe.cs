using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScreenWipe : MonoBehaviour
{
    [FormerlySerializedAs("isExitWipe")] [SerializeField] bool isTransitionOut = false;
    
    [SerializeField] Vector2 displacement = new(-10f, 0f);
    Vector2 startPos;
    
    [SerializeField] float duration = 1f;
    float timer = 0f;
    
    bool doWipe = false;
    
    // Update is called once per frame
    void Start() {
        startPos = transform.position;

        if (isTransitionOut) {
            startPos -= displacement;
            transform.position = startPos;
            StartWipe();
        }
    }
    
    void Update() {
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
