using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallBounceSound : MonoBehaviour
{
    public AudioClip bounceClip;          
    public float minImpactSpeed = 0.5f;   
    public float bounceCooldown = 0.05f;  

    private AudioSource audioSource;
    private float lastBounceTime;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Racket")) return;

        float speed = collision.relativeVelocity.magnitude;
        if (speed < minImpactSpeed) return;
        if (Time.time - lastBounceTime < bounceCooldown) return;

        lastBounceTime = Time.time;
        float volume = Mathf.Clamp01(speed / 15f);  
        audioSource.PlayOneShot(bounceClip, volume);
    }
}