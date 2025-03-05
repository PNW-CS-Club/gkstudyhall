using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour {
    [ SerializeField ] Image abilImg;
    [ SerializeField ] Image backgroundImg;

    float alpha = 0f; // ranges from 0 to 1
    const float backGroundMaxAlpha = 0.75f;
    [ SerializeField ] float fadeTime = 0.5f;
    [ SerializeField ] float growScale = 0.04f; // this had to be so tiny because it wasn't adjusted by Time.deltaTime (the FPS is ~400)
    [ SerializeField ] float timeShown = 2.5f; // this INCLUDES fading in AND out, so
    // default settings 2.5 = fade in for 0.5, hold for 1.5, fade out for 0.5

    float startTime = 0f;

    void OnEnable() {
        this.alpha = 0f;
        this.startTime = Time.time;
        this.abilImg.transform.localScale = Vector3.one;
    }

    void Start() {
        this.abilImg.color = Color.white;
        this.backgroundImg.color = new( 0f, 0f, 0f, 0f );
    }

    void Update()
    {
        var alphaChange = Time.deltaTime / fadeTime;
        var isFadingOut = Time.time >= this.startTime + ( timeShown - fadeTime );
        alpha = Mathf.Clamp01(isFadingOut ? alpha - alphaChange : alpha + alphaChange);

        this.abilImg.color = new( 1f, 1f, 1f, alpha );
        this.abilImg.transform.localScale += new Vector3( 1f, 1f, 0f ) * ( growScale * Time.deltaTime );
        this.backgroundImg.color = new( 0f, 0f, 0f, alpha * backGroundMaxAlpha );

        if ( Time.time >= this.startTime + timeShown ) {
            this.gameObject.SetActive( false );
        }
    }
}
