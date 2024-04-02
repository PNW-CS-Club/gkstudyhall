using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    bool isRolling = false;
    bool isFinishing = false;

    [SerializeField]
    float maxAngularVel = 720f;
    [SerializeField]
    float minAngularVel = 360f;

    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    float animateTimer = 0f;
    [SerializeField, Range(0f, 1f)]
    float animateInterval = 0.1f;

    float finishTimer = 0f;
    [SerializeField, Range(0f, 2f)]
    float finishDuration = 0.75f;

    [SerializeField, Range(0, 0.5f)]
    float frictionFactor = 0.03f;

    [SerializeField]
    float shakeSpeed = 10f;
    [SerializeField]
    float shakeRadius = 1f;
    [SerializeField]
    float releaseMultiplier = 5f;

    [SerializeField]
    Vector2 topLeftBound;
    [SerializeField]
    Vector2 bottomRightBound;

    [SerializeField]
    GameObject barrier;

    int roll = -1;

    private void Start() {
        if (sprites.Length != 6) {
            Debug.LogError($"There should be 6 sprites in DiceRoll array (actual: {sprites.Length})");
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody2D>();
        barrier.SetActive(false);
    }

    void Update() {

        if (isRolling) {

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;

            while (animateTimer >= animateInterval) {
                spriteRenderer.sprite = sprites[Random.Range(0, 6)];

                float unitCircleAngle = Random.Range(0f, 360f);
                Vector3 unitCircleVec = new Vector3(Mathf.Cos(unitCircleAngle), Mathf.Sin(unitCircleAngle), 0);
                Vector3 targetPos = unitCircleVec * shakeRadius;
                Vector3 newVel = targetPos - rb.transform.localPosition;
                newVel.z = 0;
                rb.velocity = newVel.normalized * shakeSpeed;

                float angVel = Random.Range(minAngularVel, maxAngularVel);
                if (Random.value <= 0.5f) {
                    angVel = -angVel;
                }
                rb.angularVelocity = angVel;

                animateTimer -= animateInterval;
            }

            animateTimer += Time.deltaTime;
        }

        else if (isFinishing) {

            rb.angularVelocity *= Mathf.Pow(frictionFactor, Time.deltaTime);
            rb.velocity *= Mathf.Pow(frictionFactor, Time.deltaTime);

            if (finishTimer >= finishDuration) {
                rb.angularVelocity = 0;
                rb.velocity = Vector2.zero;
                Debug.Log($"You rolled a {roll}.");

                transform.position = rb.transform.position;
                rb.transform.localPosition = Vector2.zero;

                barrier.SetActive(false);

                isFinishing = false;
            }

            Vector2 clampedPosition = rb.transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, topLeftBound.x, bottomRightBound.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomRightBound.y, topLeftBound.y);
            rb.transform.position = clampedPosition;

            finishTimer += Time.deltaTime;
        }
    }

    public void MouseDownFunc() {
        if (!isFinishing) {
            isRolling = true;
            animateTimer = animateInterval;
        }
    }

    public void MouseUpFunc() {
        isRolling = false;
        isFinishing = true;
        finishTimer = 0;

        roll = Random.Range(1, 7);
        spriteRenderer.sprite = sprites[roll - 1];
        rb.velocity *= releaseMultiplier;

        barrier.SetActive(true);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(topLeftBound, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(bottomRightBound, 0.5f);
    }
}
