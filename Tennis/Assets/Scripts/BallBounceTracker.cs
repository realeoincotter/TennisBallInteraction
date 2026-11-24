using UnityEngine;

public class BallBounceTracker : MonoBehaviour
{
    public Transform net;       // drag tennisNet here
    public Transform player;    // drag XR Origin (XR Rig) here
    public string groundTag = "Ground";
    public string racketTag = "Racket";
    public float minTimeBetweenBounces = 0.15f;

    int bounces = 0;
    float lastBounceTime = -1f;
    bool inPlay = false;
    Vector3 playerSideDir;

    void Start()
    {
        if (!net || !player) Debug.LogError("BallBounceTracker: Net or Player slot is empty!");
        if (!GameManager.Instance) Debug.LogError("BallBounceTracker: No GameManager in the scene!");

        playerSideDir = player.position - net.position;
        playerSideDir.y = 0;
        playerSideDir.Normalize();
    }

    // Racket with a normal collider
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ball touched: " + collision.collider.name + " (tag: " + collision.collider.tag + ")");

        if (IsRacket(collision.collider)) { RacketHit(); return; }
        if (collision.collider.CompareTag(groundTag)) GroundBounce();
    }

    // Racket with a trigger collider
    void OnTriggerEnter(Collider other)
    {
        if (IsRacket(other)) RacketHit();
    }

    bool IsRacket(Collider c)
    {
        return c.CompareTag(racketTag) || c.GetComponentInParent<RacketHit>() != null;
    }

    void RacketHit()
    {
        bounces = 0;
        inPlay = true;
        Debug.Log("Racket hit - bounce count reset");
    }

    void GroundBounce()
    {
        if (GameManager.Instance.IsGameOver || !inPlay) return;
        if (Time.time - lastBounceTime < minTimeBetweenBounces) return;
        lastBounceTime = Time.time;

        if (IsOnPlayerSide(transform.position))
        {
            bounces++;
            Debug.Log("Bounce on player side: " + bounces);
            if (bounces >= 2) GameManager.Instance.EndGame();
        }
        else
        {
            Debug.Log("Bounce on far side (doesn't count)");
        }
    }

    bool IsOnPlayerSide(Vector3 point)
    {
        Vector3 fromNet = point - net.position;
        fromNet.y = 0;
        return Vector3.Dot(fromNet, playerSideDir) > 0;
    }
}