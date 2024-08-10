using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DiceRoll : MonoBehaviour
{
    bool isHeld = false;
    bool isSliding = false;


    [Header("Shaking")]
    [SerializeField, Range(0f, 1f)] float shakeInterval = 0.1f;
    float shakeTimer = 0f;

    // angular velocities the dice is set to while held
    [SerializeField] float maxAngularVel = 1440f;
    [SerializeField] float minAngularVel = 900f;

    [SerializeField] float shakeSpeed = 3f;
    [SerializeField] float shakeRadius = 0.25f;


    [Header("Sliding")]
    [SerializeField] float releaseMultiplier = 12f;
    [SerializeField, Range(0, 0.1f)] float frictionFactor = 0.002f;

    float slideTimer = 0f;
    [SerializeField, Range(0f, 1.5f)] float slideDuration = 0.75f;


    [Header("Boundaries")]
    // the dice is restricted between these while sliding
    [SerializeField] Vector2 topLeftBound;
    [SerializeField] Vector2 bottomRightBound;

    [SerializeField] GameObject barrier;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    [Space]
    [SerializeField] Sprite[] sprites; // the 6 dice faces
    StateMachine stateMachine;


    int roll = -1;
    public UnityEvent<int> endEvent; // functions to be called after the dice is done rolling


    void Start() {
        if (sprites.Length != 6) {
            Debug.LogError($"There should be 6 sprites in DiceRoll array (actual: {sprites.Length})");
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody2D>();
        barrier.SetActive(false);
    }


    public void Initialize(StateMachine stateMachine) {
        this.stateMachine = stateMachine;
        endEvent.AddListener(_ => stateMachine.TransitionTo(stateMachine.choosingGateState));
    }


    void Update() {
        if (isHeld) {
            UpdateHeldDice();
        } 

        else if (isSliding) {
            UpdateSlidingDice();
        }
    }


    // called whenever the dice is clicked on
    public void MouseDownFunc() {
        // we will likely have to change the condition here to account for
        // all the times you are and aren't allowed to pick up the dice
        if (!isSliding) {
            isHeld = true;
            shakeTimer = shakeInterval;
        }
    }


    private void UpdateHeldDice() {

        // move the dice base to the mouse's x and y position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = transform.position.z;
        transform.position = mousePosition;

        // shake the dice once every `shakeInterval` seconds
        if (shakeTimer >= shakeInterval) {

            // change the face to show a random number
            spriteRenderer.sprite = sprites[Random.Range(0, 6)];

            // get a random point `shakeRadius` units away from the mouse,
            // then start moving the dice sprite towards it
            float unitCircleAngle = Random.Range(0f, 360f);
            Vector3 unitCircleVec = new Vector3(Mathf.Cos(unitCircleAngle), Mathf.Sin(unitCircleAngle), 0);
            Vector3 targetPos = unitCircleVec * shakeRadius;
            Vector3 newVel = targetPos - rb.transform.localPosition;
            newVel.z = 0;
            rb.velocity = newVel.normalized * shakeSpeed;

            // randomize angular velocity
            float angVel = Random.Range(minAngularVel, maxAngularVel);
            if (Random.value <= 0.5f) {
                angVel = -angVel;
            }
            rb.angularVelocity = angVel;

            // un-tick the timer
            shakeTimer -= shakeInterval;
        }

        // tick the timer
        shakeTimer += Time.deltaTime;
    }


    // called whenever the dice stops being clicked on
    public void MouseUpFunc() {
        if (!isHeld) {
            return;
        }

        isHeld = false;
        isSliding = true;
        slideTimer = 0;

        // determines the roll that this dice will get
        roll = Random.Range(1, 7);
        spriteRenderer.sprite = sprites[roll - 1];

        // gives the dice a boost
        rb.velocity *= releaseMultiplier;

        // restrict the dice's position to inside the screen bounds
        ClampDice();
        barrier.SetActive(true);
    }


    private void UpdateSlidingDice() {
        // apply friction
        rb.angularVelocity *= Mathf.Pow(frictionFactor, Time.deltaTime);
        rb.velocity *= Mathf.Pow(frictionFactor, Time.deltaTime);

        ClampDice();

        if (slideTimer >= slideDuration) {
            // finish sliding

            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;

            // shift this transform to be directly under the dice sprite's transform
            transform.position = rb.transform.position;
            rb.transform.localPosition = Vector2.zero;

            barrier.SetActive(false);

            isSliding = false;

            // call some function after the dice is done rolling
            endEvent.Invoke(roll);
        }

        slideTimer += Time.deltaTime;
    }


    // restricts the dice's position so that it is between topLeftBound and bottomRightBound
    private void ClampDice() {
        Vector2 clampedPosition = rb.transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, topLeftBound.x, bottomRightBound.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomRightBound.y, topLeftBound.y);
        rb.transform.position = clampedPosition;
    }



    // some debug code to show where the dice teleport bounds are when this gameobject is selected
#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        float gizmoRadius = 0.25f;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topLeftBound, gizmoRadius);
        Handles.Label(topLeftBound + Vector2.right * gizmoRadius, "Upper Left Bound");

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bottomRightBound, gizmoRadius);
        Handles.Label(bottomRightBound + Vector2.right * gizmoRadius, "Bottom Right Bound");
    }
#endif
}
