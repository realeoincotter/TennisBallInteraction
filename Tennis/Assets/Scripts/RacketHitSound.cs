using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RacketHitSound : MonoBehaviour
{
    public AudioClip hitClip;          // optional; falls back to the AudioSource's clip
    public float minImpactSpeed = 0.5f; // ignore tiny touches

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        float speed = collision.relativeVelocity.magnitude;
        if (speed < minImpactSpeed) return;

        // Louder for harder hits
        float volume = Mathf.Clamp01(speed / 20f);
        audioSource.PlayOneShot(hitClip != null ? hitClip : audioSource.clip, volume);
    }
}