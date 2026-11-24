using UnityEngine;

public class RacketHit : MonoBehaviour
{
    public Transform hitPoint;        // empty object at the centre of the strings
    public float hitPower = 1.2f;     // how much swing speed carries into the ball
    public float minHitSpeed = 4f;    // even a gentle tap sends it this fast
    public float maxHitSpeed = 25f;   // stops crazy shots
    public float upwardLift = 0.15f;  // small upward angle to clear the net
    public float hitCooldown = 0.2f;  // stops one swing hitting twice

    Vector3 lastPos;
    Vector3 racketVelocity;
    float lastHitTime = -1f;

    void Start() => lastPos = hitPoint.position;

    void Update()
    {
        // Measure how fast the racket head is moving (smoothed to reduce jitter)
        if (Time.deltaTime > 0)
        {
            Vector3 raw = (hitPoint.position - lastPos) / Time.deltaTime;
            racketVelocity = Vector3.Lerp(racketVelocity, raw, 0.5f);
        }
        lastPos = hitPoint.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;
        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        Rigidbody ball = other.attachedRigidbody;
        if (!ball) return;

        // Pick which side of the strings the ball is going out of
        Vector3 faceNormal = hitPoint.forward;
        if (racketVelocity.magnitude > 0.5f)
        {
            if (Vector3.Dot(faceNormal, racketVelocity) < 0) faceNormal = -faceNormal;
        }
        else if (Vector3.Dot(faceNormal, ball.linearVelocity) > 0)
        {
            faceNormal = -faceNormal; // racket still: bounce the ball back the way it came
        }

        // Ball goes mostly where the strings point, a bit towards the swing direction
        Vector3 dir = Vector3.Lerp(faceNormal, racketVelocity.normalized, 0.3f);
        dir += Vector3.up * upwardLift;
        dir.Normalize();

        float speed = Mathf.Clamp(racketVelocity.magnitude * hitPower, minHitSpeed, maxHitSpeed);

        ball.linearVelocity = dir * speed;
        ball.angularVelocity = Vector3.zero;
    }
}