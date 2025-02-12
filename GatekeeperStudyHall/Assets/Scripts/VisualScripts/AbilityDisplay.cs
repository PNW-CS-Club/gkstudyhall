using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour {
    [ SerializeField ] Image abilImg;
    [ SerializeField ] Image backgroundImg;

    float alpha = 0f;
    const float fadeInTime = 0.5f;
    const float growScale = 0.0001f; // be careful with this field, this very small value might already be too much
    public static readonly float timeShown = 2.5f;

    float startTime = 0f;

    void OnEnable() {
        this.alpha = 0f;
        this.startTime = Time.time;
        this.abilImg.transform.localScale = new Vector3( 1f, 1f, 1f );
    }

    // Start is called before the first frame update
    void Start() {
        this.abilImg.color = new( 1f, 1f, 1f, 1f );
        this.backgroundImg.color = new( 0f, 0f, 0f, 0f );
    }

    // Update is called once per frame
    void Update() {
        if ( Time.time >= this.startTime + ( timeShown - fadeInTime ) ) {
            alpha = Mathf.Clamp( alpha - ( ( 192f / fadeInTime ) * Time.deltaTime ), 0f, 192f );
        } else {
            alpha = Mathf.Clamp( alpha + ( ( 192f / fadeInTime ) * Time.deltaTime ), 0f, 192f );
        }

        this.abilImg.color = new( 1f, 1f, 1f, ( alpha / 255f ) + 0.25f );
        this.abilImg.transform.localScale += new Vector3( growScale, growScale, 0f );
        this.backgroundImg.color = new( 0f, 0f, 0f, alpha / 255f );

        if ( Time.time >= this.startTime + timeShown ) {
            this.gameObject.SetActive( false );
        }
    }
}
