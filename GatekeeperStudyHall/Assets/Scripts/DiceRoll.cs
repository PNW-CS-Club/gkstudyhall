using System.Collections.Generic;
using System.Linq;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This MonoBehavior is in charge of making the dice animate when the user interacts with it
/// and triggering the relevant code when the dice is done rolling.
/// </summary>
public class DiceRoll : MonoBehaviour
{
    bool isHeld = false;
    bool isSliding = false;
    bool isReturning = false;
    public bool canCheatRolls = false;

    [SerializeField] GameManager gameManager;
    [SerializeField] Sprite[] sprites; // the 6 dice faces


    [Header("Shaking")]
    [SerializeField, Range(0f, 1f)] float shakeInterval = 0.1f;
    float shakeTimer = 0f;

    // angular velocities the dice is set to while held
    [SerializeField] float maxAngularVel = 1440f;
    [SerializeField] float minAngularVel = 900f;

    [SerializeField] float shakeSpeed = 3f;
    [SerializeField] float shakeRadius = 0.25f;


    [Header("Sliding")]
    [SerializeField] float throwSpeedMultiplier = 2.5f;
    [SerializeField] float lowSpeedThreshold = 10f;
    [SerializeField] float slidingWindowDuration = 0.1f;
    [SerializeField, Range(0, 0.1f)] float frictionFactor = 0.002f;
    
    float slideTimer = 0f;
    // this controls how long the dice is stopped after rolling
    // it includes a brief pause before resetting, so consider that when setting the value
    [SerializeField, Range(0f, 3f)] float slideDuration = 1.25f;
    
    [Header("Returning")]
    [SerializeField, Min(0)] float returnDuration = 0.5f;
    float returnTimer = 0f;
    [SerializeField] AnimationCurve returnCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    Vector3 stopPosition;
    float stopAngle;

    [Header("Boundaries")]
    // the dice is restricted between these while sliding
    [SerializeField] Vector2 topLeftBound;
    [SerializeField] Vector2 bottomRightBound;

    [SerializeField] GameObject barrier;
    SpriteRenderer diceNum;
    SpriteRenderer diceBackground;
    Rigidbody2D rb;
    
    public static PlayerSO owner;

    Vector3 startPosition;
    
    // stores each frame's mouseDelta and the time it was calculated
    readonly Queue<(Vector2, float)> prevMouseDeltas = new();
    Vector2 avgMouseDelta;
    float avgMouseDistance;
    
    [HideInInspector] public event System.EventHandler<int> DoneRollingEvent;
    int roll = -1;


    void Start() {
        startPosition = transform.position;
        if (sprites.Length != 6) {
            Debug.LogError($"There should be 6 sprites in DiceRoll array (actual: {sprites.Length})");
        }

        // this is stupid but i don't see any docs specifying which order elements are returned in,
        // so i have to assume it's just arbitrary
        SpriteRenderer[] diceImages = GetComponentsInChildren<SpriteRenderer>();
        if ( diceImages[ 0 ].name == "Dice Background" ) {
            diceBackground = diceImages[ 0 ];
            diceNum = diceImages[ 1 ];
        } else {
            diceBackground = diceImages[ 1 ];
            diceNum = diceImages[ 0 ];
        }
        rb = GetComponentInChildren<Rigidbody2D>();
        barrier.SetActive(false);

        prevMouseDeltas.Enqueue((Vector2.zero, Time.time));
    }
    
        
    void Update() {
        prevMouseDeltas.Enqueue((new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Time.time));
        // Dequeue expired deltas unless there are only 3 deltas left
        while (prevMouseDeltas.Count > 3 && Time.time >= prevMouseDeltas.Peek().Item2 + slidingWindowDuration)
            prevMouseDeltas.Dequeue();
        
        var mouseSum = prevMouseDeltas
            .Select(x => x.Item1)
            .Aggregate(Vector2.zero, (current, next) => current + next);
        avgMouseDelta = mouseSum / prevMouseDeltas.Count; 
        
        var mouseDistanceSum = prevMouseDeltas
            .Select(x => x.Item1)
            .Aggregate(0f, (current, next) => current + next.magnitude);
        avgMouseDistance = mouseDistanceSum / prevMouseDeltas.Count;
        
        if (canCheatRolls)
            TryCheating();
        
        if (isHeld) {
            UpdateHeldDice();
        } 
        else if (isSliding) {
            bool isDone = UpdateSlidingDice();

            if (isDone) {
                isSliding = false;
                DoneRollingEvent?.Invoke(this, roll);
                
                // this code below makes the dice return IMMEDIATELY after it stops rolling
                isReturning = true;
                returnTimer = returnDuration;
                stopPosition = transform.position;
                stopAngle = diceNum.transform.eulerAngles.z;
            }
        } 
        else if (isReturning) {
            returnTimer -= Time.deltaTime;
            returnTimer = Mathf.Max(0f, returnTimer);
            
            float t = Mathf.InverseLerp(returnDuration, 0f, returnTimer);
            float progress = returnCurve.Evaluate(t);
            transform.position = Vector3.Lerp(stopPosition, startPosition, progress);

            // round to nearest 180-degree rotation (the dice has 180-degree symmetry)
            float targetAngle = Mathf.Round(stopAngle / 180f) * 180f;
            // whats a quarternion
            float zRot = Mathf.Lerp(stopAngle, targetAngle, progress);
            diceNum.transform.eulerAngles = new( 0f, 0f, zRot );

            if (returnTimer <= 0f) {
                isReturning = false;
            }
        }

        diceBackground.transform.SetPositionAndRotation( diceNum.transform.position, diceNum.transform.rotation );

        diceBackground.color = owner.card.outerColor;
        diceNum.color = owner.card.detailColor;
    }


    /// Method called whenever the user mouses down on the dice.
    /// If the user is allowed to pick up the dice now, it begins shaking.
    public void MouseDownFunc()
    {
        if (!gameManager.currentState.CanRoll() || isSliding) return;
        
        isHeld = true;
        shakeTimer = shakeInterval;
    }


    /// Continues the process of shaking the dice.
    private void UpdateHeldDice() {

        // move the dice base to the mouse's x and y position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = transform.position.z;
        transform.position = mousePosition;

        // shake the dice once every `shakeInterval` seconds
        if (shakeTimer >= shakeInterval) {

            // change the face to show a random number
            diceNum.sprite = sprites[Random.Range(0, 6)];

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


    /// Method called whenever the user mouses up on the dice.
    public void MouseUpFunc() {
        if (isHeld)
            ReleaseDice(Random.Range(0, 6) + 1);
    }

    
    void TryCheating()
    {
        if (!gameManager.currentState.CanRoll() || isSliding) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ReleaseDice(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ReleaseDice(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ReleaseDice(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ReleaseDice(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ReleaseDice(5);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ReleaseDice(6);
    }


    /// <summary>
    /// Stops shaking the dice, displays the determined roll, and begins to slide the dice.
    /// </summary>
    /// <param name="finalRoll">The value this dice roll will produce</param>
    private void ReleaseDice(int finalRoll)
    {
        isHeld = false;
        isSliding = true;
        slideTimer = 0;

        roll = finalRoll;
        diceNum.sprite = sprites[finalRoll - 1];

        // gives the dice a boost
        var avgMouseSpeed = avgMouseDistance / Time.fixedDeltaTime;
        var avgMouseVel = avgMouseDelta.normalized * avgMouseSpeed;
        
        if ( avgMouseSpeed <= lowSpeedThreshold ) {
            // if you're lazy and aren't shaking the die very much, boost it to reach some speed
            var effort = avgMouseSpeed / lowSpeedThreshold;
            // the less effort you put in, the more weight the random direction will have
            var newDirection = Random.insideUnitCircle * (1 - effort) + avgMouseVel.normalized * effort;
            rb.velocity = newDirection.normalized * (lowSpeedThreshold * throwSpeedMultiplier);
        }
        else {
            rb.velocity = avgMouseVel * throwSpeedMultiplier;
        }

        // restrict the dice's position to inside the screen bounds
        ClampDice();
        barrier.SetActive(true);
    }


    /// <summary>
    /// Continues to slide the dice until <c>slideDuration</c> has passed.
    /// If it has, stops sliding and cleans up.
    /// </summary>
    /// <returns><c>true</c> only if the dice is done sliding.</returns>
    private bool UpdateSlidingDice() {
        // apply friction
        rb.angularVelocity *= Mathf.Pow(frictionFactor, Time.deltaTime);
        rb.velocity *= Mathf.Pow(frictionFactor, Time.deltaTime);

        ClampDice();

        if ( slideTimer >= slideDuration ) {
            CleanUpAfterRoll();
            return true;
        }

        slideTimer += Time.deltaTime;
        return false;
    }


    private void CleanUpAfterRoll() 
    {
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;

        // shift this transform to be directly under the dice sprite's transform
        transform.position = rb.transform.position;

        rb.transform.localPosition = Vector2.zero;

        barrier.SetActive(false);
        isSliding = false;
    }


    /// <summary>
    /// Restricts the dice's position so that it is inside the rectangle
    /// formed by <c>topLeftBound</c> and <c>bottomRightBound</c>
    /// </summary>
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
