using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CenterGateStyle : MonoBehaviour {
    [ SerializeField ] CenterGateSO gate;
    [ SerializeField ] Image hpImage;

    private Sprite[] spriteSheet;
    private readonly List< Sprite > spriteList = new();

    // Start is called before the first frame update
    void Start() {
        spriteSheet = Resources.LoadAll< Sprite >( "hp_sheet" );

        for ( int i = 1; i <= 10; i++ ) {
            string str = "hp_sheet_" + i;

            //Sprite fireBallSprite = abilityIconsAtlas.Single(s => s.name == "FireBall");
            spriteList.Add( spriteSheet.Single( s => s.name == str ) );
        }
    }

    // Update is called once per frame
    void Update() {
        hpImage.sprite = spriteList[ gate.Health - 1 ];
    }
}
