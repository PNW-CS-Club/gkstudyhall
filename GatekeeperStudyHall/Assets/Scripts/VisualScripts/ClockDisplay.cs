using System;
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

        int numHours = ( int )timeElapsed / ( 60 * 60 );
        int numMins = ( int )( timeElapsed / 60 ) % 60;
        int numSecs = ( int )( timeElapsed % 60 );

        if ( timeElapsed >= 60f * 60f ) {
            textObj.text = String.Format( "{0:D2}:{1:D2}:{2:D2}", numHours, numMins, numSecs );
        } else {
            textObj.text = String.Format( "{0:D2}:{1:D2}", numMins, numSecs );
        }
    }

    public void ResetTime() {
        this.startTime = Time.time;
    }
}
