using UnityEngine;

public class BullseyePull : MonoBehaviour
{
    public float pullRadius = 2f;     // how far away the pull starts (metres)
    public float pullStrength = 15f;  // how strong the pull is at the centre

    Rigidbody ball;
    Vector3 centre;

    void Start()
    {
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj) ball = ballObj.GetComponent<Rigidbody>();

        // Use the visual centre of the mesh, in case the pivot isn't centred
        Renderer r = GetComponentInChildren<Renderer>();
        centre = r ? r.bounds.center : transform.position;
    }

    void FixedUpdate()
    {
        if (!ball) return;

        Vector3 toCentre = centre - ball.position;
        float distance = toCentre.magnitude;
        if (distance > pullRadius) return;

        // Only pull while the ball is heading towards the target,
        // so it doesn't drag the ball back after it bounces off
        if (Vector3.Dot(ball.linearVelocity, toCentre) <= 0) return;

        // Stronger the closer it gets
        float strength = pullStrength * (1f - distance / pullRadius);
        ball.AddForce(toCentre.normalized * strength, ForceMode.Acceleration);
    }

    // Shows the pull area as a sphere in the Scene view when selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Renderer r = GetComponentInChildren<Renderer>();
        Gizmos.DrawWireSphere(r ? r.bounds.center : transform.position, pullRadius);
    }
}