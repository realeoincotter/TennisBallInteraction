using UnityEngine;

public class BallAimAssist : MonoBehaviour
{
    public Transform racketHitPoint;   
    public Transform player;          
    public float assistRange = 1.5f;   
    public float assistStrength = 3f;  
    public float maxSpeed = 8f;        

    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        Vector3 toRacket = racketHitPoint.position - rb.position;
        bool comingTowardPlayer = Vector3.Dot(rb.linearVelocity, player.position - rb.position) > 0;

        if (!comingTowardPlayer || toRacket.magnitude > assistRange) return;

        // Bend the ball's path toward the racket, keeping its speed
        float speed = Mathf.Min(rb.linearVelocity.magnitude, maxSpeed);
        Vector3 desired = toRacket.normalized * speed;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desired, assistStrength * Time.fixedDeltaTime);
    }
}