using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayerSlots : MonoBehaviour
{
    [SerializeField] float heightVariation = 1f;
    [SerializeField] float horizontalSpacing = 425f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float duration = 4f;
    [SerializeField] Transform plusButton;
    [SerializeField] float plusButtonOffset = 425f;
    
    readonly Transform[] slots = new Transform[4];
    float t = 0f;
    int numSlotsVisible = 2;

    const float FullTurn = 2 * Mathf.PI;
    const float QuarterTurn = Mathf.PI / 2;
    
    void Start()
    {
        for (int i = 0; i < slots.Length; i++) {
            slots[i] = transform.GetChild(i);
            if (i >= numSlotsVisible) {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        t += Time.deltaTime / duration;

        float totalXDistance = horizontalSpacing * (numSlotsVisible - 1);
        if (numSlotsVisible < 4) 
            totalXDistance += plusButtonOffset;
        float startX = totalXDistance * -0.5f;

        // reposition each card slot
        for (int i = 0; i < slots.Length; i++) {
            var localPos = slots[i].localPosition;
            
            float targetX = startX + i * horizontalSpacing;
            float deltaX = targetX - localPos.x;
            // exponential decay from localPos.x to targetX
            // localPos.x += Δx * (|Δx|^(speed*dt) - 1)
            localPos.x += deltaX * (Mathf.Pow(Mathf.Abs(deltaX), horizontalSpeed * Time.deltaTime) - 1);
            
            // bob up and down with sine function
            localPos.y = Mathf.Sin(t*FullTurn - i*QuarterTurn) * heightVariation;
            
            slots[i].localPosition = localPos;
        }
        
        // reposition the button
        {
            var localPos = plusButton.localPosition;
                
            float targetX = startX + (numSlotsVisible-1) * horizontalSpacing + plusButtonOffset;
            float deltaX = targetX - localPos.x;
            localPos.x += deltaX * (Mathf.Pow(Mathf.Abs(deltaX), horizontalSpeed * Time.deltaTime) - 1);
            
            plusButton.localPosition = localPos;
        }
    }

    public void AddSlot() {
        if (numSlotsVisible == 4) return;
        
        numSlotsVisible++;
        transform.GetChild(numSlotsVisible-1).gameObject.SetActive(true);

        if (numSlotsVisible == 4) {
            plusButton.gameObject.SetActive(false);
        }
    }

    public void RemoveSlot() {
        if (numSlotsVisible == 2) return;
        
        numSlotsVisible--;
        transform.GetChild(numSlotsVisible).gameObject.SetActive(false);
        
        if (numSlotsVisible < 4) {
            plusButton.gameObject.SetActive(true);
        }
    }
}
