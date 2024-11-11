using UnityEngine;

public class Box : MonoBehaviour
{
    public int points = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ScoringSystem scoringSystem = FindObjectOfType<ScoringSystem>();
            if (scoringSystem != null)
            {
                scoringSystem.AddScore(points);
            }
            Destroy(gameObject);
        }
    }
}
