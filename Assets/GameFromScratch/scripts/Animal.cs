using UnityEngine;

public class Animal : MonoBehaviour
{
    public int points = 10; // Points to add when hit by a bullet
    public float lifetime = 10f; // Time in seconds before the animal is destroyed automatically
    public AudioClip creationSound; // Sound to play when the animal is created
    public AudioClip hitSound; // Sound to play when the animal is hit
    public GameObject destructionParticles; // Particle system prefab to instantiate

    void Start()
    {
        // Destroy the animal after its lifetime expires
        Destroy(gameObject, lifetime);

        // Play the creation sound when the animal is instantiated
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
            Destroy(gameObject); // Destroy the animal
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
