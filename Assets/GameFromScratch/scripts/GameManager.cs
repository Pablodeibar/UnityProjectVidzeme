using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Add this line

public class GameManager : MonoBehaviour
{
    public Text introText;
    public Text scoreText;  // Reference to ScoreText
    public ScoringSystem scoringSystem;
    public GameObject gameEndPanel;
    public int targetScore = 50; // Score required to end the game

    void Start()
    {
        // Hide score text at the start
        scoreText.gameObject.SetActive(false);
        // Show intro text at the start
        StartCoroutine(ShowIntroMessage());
        // Hide game end panel at the start
        gameEndPanel.SetActive(false);
    }

    IEnumerator ShowIntroMessage()
    {
        introText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f); // Show intro for 5 seconds
        introText.gameObject.SetActive(false);
        // Ensure the intro text stays hidden
        introText.gameObject.SetActive(false);
        // Show score text after intro message disappears
        scoreText.gameObject.SetActive(true);
    }

    void Update()
    {
        // Check if the target score is reached
        if (scoringSystem.score >= targetScore)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        // Display game end message
        gameEndPanel.SetActive(true);
        // Reload the scene after a delay
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        // Wait for a few seconds before restarting
        yield return new WaitForSeconds(5f);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
