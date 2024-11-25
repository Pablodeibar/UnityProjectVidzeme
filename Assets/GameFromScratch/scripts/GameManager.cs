using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text introText;
    public Text scoreText;
    public Text levelText;
    public Text achievementText;
    public ScoringSystem scoringSystem;
    public GameObject gameEndPanel;
    public AudioClip endGameSound;
    public AudioClip levelUpSound;
    public string introSceneName = "IntroScene"; // Name of the intro scene

    private AudioSource audioSource;
    private int currentLevel = 0;
    private int[] targetScores = { 10, 10, 10 };

    private enum GameState { Intro, Playing, End }
    private GameState currentState;

    void Start()
    {
        InitializeComponents();
        currentState = GameState.Intro;
        StartCoroutine(ShowIntroMessage());
    }

    void InitializeComponents()
    {
        audioSource = GetComponent<AudioSource>();

        // Dynamically find UI elements and scoring system
        introText = GameObject.Find("IntroText")?.GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
        levelText = GameObject.Find("LevelText")?.GetComponent<Text>();
        achievementText = GameObject.Find("AchievementsText")?.GetComponent<Text>();
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
        gameEndPanel = GameObject.Find("GameEndPanel");

        Debug.Log("Initializing components.");
        scoreText?.gameObject.SetActive(false);
        levelText?.gameObject.SetActive(false);
        achievementText?.gameObject.SetActive(false);
        if (gameEndPanel != null)
        {
            gameEndPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("GameEndPanel not found in the scene.");
        }
    }

    IEnumerator ShowIntroMessage()
    {
        Debug.Log("Showing intro message.");
        introText?.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f); // Show intro for 5 seconds
        introText?.gameObject.SetActive(false);
        scoreText?.gameObject.SetActive(true);
        levelText?.gameObject.SetActive(true);
        achievementText?.gameObject.SetActive(true);
        currentState = GameState.Playing;
        UpdateLevelUI();
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        // Check all references before use
        if (scoringSystem == null || scoreText == null || levelText == null || achievementText == null) return;

        // Debug log to track the score
        Debug.Log("Current Score: " + scoringSystem.score);

        if (scoringSystem.score >= targetScores[currentLevel])
        {
            if (currentLevel < targetScores.Length - 1)
            {
                LevelUp();
            }
            else
            {
                EndGame();
            }
        }
    }

    void LevelUp()
    {
        Debug.Log("Leveling Up: Current Level = " + currentLevel);
        currentLevel++;
        scoringSystem.score = 0; // Reset the score
        UpdateLevelUI();
        PlayLevelUpSound();
    }

    void UpdateLevelUI()
    {
        Debug.Log("Updating UI: Current Level = " + currentLevel);
        if (levelText != null)
        {
            levelText.text = "Level: " + (currentLevel + 1);
        }
        if (achievementText != null)
        {
            achievementText.text = currentLevel < targetScores.Length - 1
                ? "Points to Next Level: " + targetScores[currentLevel]
                : "Final Level Achieved!";
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoringSystem.score;
        }
        // Debug log to track UI update
        Debug.Log("Updated UI - Level: " + levelText?.text + " | Achievement: " + achievementText?.text + " | Score: " + scoreText?.text);
    }

    void PlayLevelUpSound()
    {
        if (levelUpSound != null && audioSource != null)
        {
            Debug.Log("Playing level up sound.");
            audioSource.PlayOneShot(levelUpSound);
        }
    }

    void EndGame()
    {
        Debug.Log("Ending Game: Current Level = " + currentLevel);
        currentState = GameState.End;

        if (gameEndPanel != null)
        {
            gameEndPanel.SetActive(true);
            Debug.Log("Game end panel set to active.");
        }
        else
        {
            Debug.LogError("Game end panel is null.");
        }
        if (endGameSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(endGameSound);
        }
        StartCoroutine(ReloadIntroScene());
    }

    IEnumerator ReloadIntroScene()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("GameManager: Reloading Intro Scene");
        SceneManager.LoadScene(introSceneName); // Load the intro scene instead of the current scene
    }
}

