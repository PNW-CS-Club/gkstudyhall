using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] StateMachine stateMachine;
    [SerializeField] Sprite[] sprites; // the 6 dice faces
    [SerializeField] TraitHandlerSO traitHandler;


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


    int roll = -1;
    bool userCanRoll = false;


    void OnEnable() 
    {
        stateMachine.StateChangedEvent += OnStateChanged;
    }

    void OnDestroy() 
    {
        stateMachine.StateChangedEvent -= OnStateChanged;
    }


    /// <summary>
    /// Adjusts whether the user is allowed to roll the dice based on the new state. 
    /// This method is called anytime the state changes. 
    /// </summary>
    /// <param name="state">The newly active state.</param>
    private void OnStateChanged(object sender, IState state) 
    {
        // magical C# construct that lets us easily check the type of the state
        userCanRoll = state switch 
        {
            // only let the user interact with the dice during these states:
            TraitRollState => true,
            AttackingGateState => true,
            //BreakingGateState => true,

            _ => false,
        };

        Debug.Log($"New state: {state.GetType()},  userCanRoll: {userCanRoll}");
    }


    void Start() {
        if (sprites.Length != 6) {
            Debug.LogError($"There should be 6 sprites in DiceRoll array (actual: {sprites.Length})");
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody2D>();
        barrier.SetActive(false);
    }


    void Update() {
        if (isHeld) {
            UpdateHeldDice();
        } 

        else if (isSliding) {
            UpdateSlidingDice();
        }
    }


    /// <summary>
    /// Method called whenever the user mouses down on the dice.
    /// If the user is allowed to pick up the dice now, it begins shaking.
    /// </summary>
    public void MouseDownFunc() 
    {
        if (userCanRoll && !isSliding) 
        {
            isHeld = true;
            shakeTimer = shakeInterval;
        }
    }


    /// <summary>
    /// Continues the process of shaking the dice.
    /// </summary>
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


    /// <summary>
    /// Method called whenever the user mouses up on the dice.
    /// Stops shaking the dice, determines its final value, and begins to slide it.
    /// </summary>
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


    /// <summary>
    /// Continues to slide the dice until <c>slideDuration</c> has passed.
    /// If it has, cleans up and calls <c>FinishRollWithValue</c>.
    /// </summary>
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

            // clean up
            barrier.SetActive(false);
            isSliding = false;

            // the dice is done rolling; transition to whatever is next
            FinishRollWithValue(roll);
        }

        slideTimer += Time.deltaTime;
    }


    /// <summary>
    /// Performs the action expected to happen at the end of the dice roll, depending on the state.
    /// </summary>
    /// <param name="roll">The rolled value of the dice.</param>
    private void FinishRollWithValue(int roll) 
    {
        if (stateMachine.CurrentState == stateMachine.traitRollState) 
        {
            if(roll <= 4){
                traitHandler.ActivateCurrentPlayerTrait(roll);
                stateMachine.TransitionTo(stateMachine.choosingGateState);
            }
            else if(roll == 6){
                //skip turn
                Debug.Log("(TODO: Implement transition to next player traitRollState)");
            }
            else{
                //player rolls a 5, initiate battle with another player
                Debug.Log("(TODO: Implement battling with another player)");
            }
        }
        else if (stateMachine.CurrentState == stateMachine.attackingGateState) 
        {
            Debug.Log($"attacking for {roll} damage (TODO: deal damage & transition to the next state)");
            //GameManager.GateChangeHealth(?, Globals.chosenGate, roll);
            //stateMachine.TransitionTo(the next state);
        }
        else 
        {
            Debug.LogError("The player should not be able to roll the dice now!");
        }
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
