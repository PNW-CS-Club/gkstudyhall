using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour {
    [ SerializeField ] Image abilImg;
    [ SerializeField ] Image backgroundImg;

    float alpha = 0f; // ranges from 0 to 1
    const float backGroundMaxAlpha = 0.75f;
    const float fadeInTime = 0.5f;
    const float growScale = 0.04f; // this had to be so tiny because it wasn't adjusted by Time.deltaTime (the FPS is ~400)
    public static readonly float timeShown = 2.5f;

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
        var alphaChange = Time.deltaTime / fadeInTime;
        var isFadingOut = Time.time >= this.startTime + ( timeShown - fadeInTime );
        alpha = Mathf.Clamp01(isFadingOut ? alpha - alphaChange : alpha + alphaChange);

        this.abilImg.color = new( 1f, 1f, 1f, alpha );
        this.abilImg.transform.localScale += new Vector3( 1f, 1f, 0f ) * ( growScale * Time.deltaTime );
        this.backgroundImg.color = new( 0f, 0f, 0f, alpha * backGroundMaxAlpha );

        if ( Time.time >= this.startTime + timeShown ) {
            this.gameObject.SetActive( false );
        }
    }
}
