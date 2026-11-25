using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRI 3.x (remove these two lines on XRI 2.x)
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BallRecall : MonoBehaviour
{
    [SerializeField] InputActionProperty recallAction;   // X button
    [SerializeField] Rigidbody ball;
    [SerializeField] XRBaseInteractor handInteractor;    // left hand's Direct/Near-Far Interactor

    [Header("Flight feel")]
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float acceleration = 30f;  // how fast it winds up
    [SerializeField] float steering = 6f;       // lower = wider, more dramatic curve
    [SerializeField] float catchDistance = 0.12f;
    [SerializeField] bool passThroughWalls = false;

    XRGrabInteractable grab;
    bool recalling;
    float speed;

    void Awake()
    {
        grab = ball.GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => StopRecall()); // grabbed mid-flight = cancel
    }

    void OnEnable()
    {
        recallAction.action.performed += OnRecall;
        recallAction.action.Enable();
    }

    void OnDisable() => recallAction.action.performed -= OnRecall;

    void OnRecall(InputAction.CallbackContext ctx)
    {
        if (grab.isSelected || handInteractor.hasSelection) return;
        recalling = true;
        speed = 0f;
        ball.useGravity = false;
        if (passThroughWalls) ball.detectCollisions = false;
    }

    void FixedUpdate()
    {
        if (!recalling) return;

        Vector3 toHand = handInteractor.transform.position - ball.position;
        float dist = toHand.magnitude;

        if (dist < catchDistance) { Catch(); return; }

        speed = Mathf.Min(speed + acceleration * Time.fixedDeltaTime, maxSpeed);
        // Never move further than the remaining distance in one step, so it can't overshoot
        float stepSpeed = Mathf.Min(speed, dist / Time.fixedDeltaTime);

        Vector3 desired = toHand.normalized * stepSpeed;
        ball.linearVelocity = Vector3.Lerp(ball.linearVelocity, desired, steering * Time.fixedDeltaTime);
    }

    void Catch()
    {
        StopRecall();
        ball.linearVelocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        ball.position = handInteractor.transform.position;

        handInteractor.interactionManager.SelectEnter(
            (IXRSelectInteractor)handInteractor, (IXRSelectInteractable)grab);
    }

    void StopRecall()
    {
        recalling = false;
        ball.useGravity = true;
        ball.detectCollisions = true;
    }
}