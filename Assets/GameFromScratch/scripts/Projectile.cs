using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int points = 10; // Points to add when hit by a bullet

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ScoringSystem scoringSystem = FindObjectOfType<ScoringSystem>();
            if (scoringSystem != null)
            {
                scoringSystem.AddScore(points);
            }
            Destroy(collision.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
