using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System;

public class CenterGateStyle : MonoBehaviour {
    [ SerializeField ] CenterGateSO gate;
    [ SerializeField ] Image hpImage;

    // this needs UnityEngine. because of ambiguity between UnityEngine.Object and System.Object
    // System is used for Math.Clamp below
    private UnityEngine.Object[] spriteSheet = new Sprite[ 10 ];
    private readonly List< Sprite > spriteList = new();

    // Start is called before the first frame update
    void Start() {
        spriteSheet = AssetDatabase.LoadAllAssetsAtPath( "Assets/Images/gate_hp.png" );

        for ( int i = 0; i <= 10; i++ ) {
            string str = "gate_hp_" + i;

            // scary downcast! but everything's okay
            spriteList.Add( ( Sprite )spriteSheet.Single( s => s.name == str ) );
        }
    }

    // Update is called once per frame
    void Update() {
        // no more errors if, for some odd reason, our HP is out of bounds
        hpImage.sprite = spriteList[ Math.Clamp( gate.Health, 0, CenterGateSO.MAX_HEALTH ) ];
    }
}
