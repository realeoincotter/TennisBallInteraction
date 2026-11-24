using UnityEngine;

public class TargetRing : MonoBehaviour
{
    public int points = 100;
    Target target;

    void Awake() => target = GetComponentInParent<Target>();

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
            target.RegisterHit(points);
    }
}