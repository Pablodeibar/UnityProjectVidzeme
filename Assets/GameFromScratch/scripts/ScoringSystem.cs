using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public int score;
    public Text scoreText;

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void ResetScore() { score = 0; Debug.Log("Score reset to 0"); }
}
