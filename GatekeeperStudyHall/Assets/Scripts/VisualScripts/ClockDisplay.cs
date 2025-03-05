using TMPro;
using UnityEngine;

public class ClockDisplay : MonoBehaviour {
    [ SerializeField ] TMP_Text textObj;

    private float startTime = 0f;

    void Start() {
        // automatically called when its parent scene GKScene is loaded
        this.startTime = Time.time;
    }

    void Update() {
        float timeElapsed = Time.time - this.startTime;

        int numHours = ( int )timeElapsed / ( 60 * 60 );
        int numMins = ( int )( timeElapsed / 60 ) % 60;
        int numSecs = ( int )( timeElapsed % 60 );

        if ( timeElapsed >= 60f * 60f ) {
            textObj.text = $"{numHours}:{numMins:D2}:{numSecs:D2}";
        } else {
            textObj.text = $"{numMins}:{numSecs:D2}";
        }
    }

    public void ResetTime() {
        this.startTime = Time.time;
    }
}
