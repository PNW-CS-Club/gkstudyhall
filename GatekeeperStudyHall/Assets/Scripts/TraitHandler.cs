using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// warning: changing enum int values here causes them to desync in the editor
public enum Trait
{
    TRAIT_A = 0,
    TRAIT_B = 1,
    TRAIT_C = 2,
}


public class TraitHandler : MonoBehaviour
{
    public Trait myTrait = Trait.TRAIT_A;

    public void ActivateTraitBehavior() {
        switch (myTrait) {
            case Trait.TRAIT_A:
                Debug.Log("Trait A");
                break;
            case Trait.TRAIT_B:
                Debug.Log("Trait B");
                break;
            case Trait.TRAIT_C:
                Debug.Log("Trait C");
                break;
            default:
                Debug.Log("Invalid Trait");
                break;
        }
    }
}
