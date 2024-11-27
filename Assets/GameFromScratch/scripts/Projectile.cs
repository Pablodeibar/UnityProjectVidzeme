using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int points = 10;
    public float lifetime = 5f;
    public AudioClip creationSound;
    public AudioClip hitSound;
    public GameObject destructionParticles;

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);

        // Play the creation sound when the projectile is instantiated
        if (creationSound != null)
        {
            PlaySound(creationSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ScoringSystem scoringSystem = FindObjectOfType<ScoringSystem>();
            if (scoringSystem != null)
            {
                scoringSystem.AddScore(points);
            }

            // Play the hit sound
            if (hitSound != null)
            {
                PlaySound(hitSound);
            }

            // Instantiate destruction particles
            if (destructionParticles != null)
            {
                Instantiate(destructionParticles, transform.position, transform.rotation);
            }

            Destroy(collision.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy the projectile
        }
    }

    void PlaySound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(soundObject, clip.length); // Destroy the sound object after the clip finishes
    }
}
