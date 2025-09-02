using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayerSlots : MonoBehaviour
{
    [SerializeField] float heightVariation = 1f;
    [SerializeField] float horizontalSpacing = 450f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float duration = 4f;
    
    Transform[] slots = new Transform[4];
    float t = 0f;
    int numSlotsVisible = 2;

    const float FullTurn = 2 * Mathf.PI;
    const float QuarterTurn = Mathf.PI / 2;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slots.Length; i++) {
            slots[i] = transform.GetChild(i);
            if (i >= numSlotsVisible) {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / duration;

        float xStart = -horizontalSpacing * (numSlotsVisible-1)/2f;

        for (int i = 0; i < slots.Length; i++) {
            var localPos = slots[i].localPosition;
            
            float targetX = xStart + i * horizontalSpacing;
            // exponential decay from localPos.x to targetX
            // Δx = x_t - x_c
            // x_c += Δx * (|Δx|^(speed*dt) - 1)
            float deltaX = targetX - localPos.x;  
            localPos.x += deltaX * (Mathf.Pow(Mathf.Abs(deltaX), horizontalSpeed * Time.deltaTime) - 1);; 
            
            localPos.y = Mathf.Sin(t*FullTurn - i*QuarterTurn) * heightVariation;
            
            slots[i].localPosition = localPos;
        }
    }

    public void AddSlot() {
        if (numSlotsVisible == 4) return;
        
        numSlotsVisible++;
        transform.GetChild(numSlotsVisible-1).gameObject.SetActive(true);
    }

    public void RemoveSlot() {
        if (numSlotsVisible == 2) return;
        
        numSlotsVisible--;
        transform.GetChild(numSlotsVisible).gameObject.SetActive(false);
    }
}
