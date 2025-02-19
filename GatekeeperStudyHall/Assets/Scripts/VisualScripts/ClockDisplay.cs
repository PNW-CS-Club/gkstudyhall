using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockDisplay : MonoBehaviour {
    [ SerializeField ] TMP_Text textObj;

    private float startTime = 0f;

    // Start is called before the first frame update
    void Start() {
        this.startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        float timeElapsed = ( Time.time - this.startTime );

        // hours is untested, but i'm not sitting for an hour until it hits it LOL
        // it should only show ##:## if < 1 hour and ##:##:## if > 1 hour
        if ( timeElapsed >= 60f * 60f ) {
            textObj.text = String.Format( "{0:D2}:{1:D2}:{2:D2}", ( int )( timeElapsed / ( 60 * 60 ) ),
                                                                  ( int )( timeElapsed / 60 ), 
                                                                  ( int )( timeElapsed % 60 ) );
        } else {
            textObj.text = String.Format( "{0:D2}:{1:D2}", ( int )( timeElapsed / 60 ), ( int )( timeElapsed % 60 ) );
        }
    }

    public void ResetTime() {
        this.startTime = Time.time;
    }
}
