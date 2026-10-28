using UnityEngine;

public class RacketHit : MonoBehaviour
{
    public float hitPower = 1.5f;   // how much of the swing speed transfers to the ball
    public float minBallSpeed = 3f; // stops weak, dead bounces

    Vector3 lastPos;
    Vector3 racketVelocity;

    void FixedUpdate()
    {
        racketVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        lastPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        Rigidbody ball = collision.rigidbody;
        Vector3 normal = collision.contacts[0].normal * -1f; // points out of the racket face

        Vector3 bounced = Vector3.Reflect(ball.linearVelocity, normal);
        Vector3 newVel = bounced + racketVelocity * hitPower;

        if (newVel.magnitude < minBallSpeed)
            newVel = newVel.normalized * minBallSpeed;

        ball.linearVelocity = newVel;
    }
}