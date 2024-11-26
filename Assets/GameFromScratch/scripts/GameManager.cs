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
    public Text tutoGunText; // Reference to TutoGunText
    public Text tutoTimeText; // Reference to TutoTimeText
    public ScoringSystem scoringSystem;
    public GameObject gameEndPanel;
    public AudioClip endGameSound;
    public AudioClip levelUpSound;
    public AudioClip tutorialSound; // Sound for tutorial texts
    public string introSceneName = "IntroScene"; // Name of the intro scene
    public Transform cameraTransform; // Reference to the VR camera/eye anchor

    private AudioSource audioSource;
    private AudioSource tutoAudioSource;
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
        tutoGunText = GameObject.Find("TutoGunText")?.GetComponent<Text>(); // Find TutoGunText
        tutoTimeText = GameObject.Find("TutoTimeText")?.GetComponent<Text>(); // Find TutoTimeText
        scoringSystem = GameObject.Find("ScoreManager")?.GetComponent<ScoringSystem>();
        gameEndPanel = GameObject.Find("GameEndPanel");

        // Set up audio sources
        if (tutoGunText != null)
        {
            tutoAudioSource = tutoGunText.gameObject.AddComponent<AudioSource>();
            tutoAudioSource.clip = tutorialSound;
        }

        Debug.Log("Initializing components.");
        scoreText?.gameObject.SetActive(false);
        levelText?.gameObject.SetActive(false);
        achievementText?.gameObject.SetActive(false);
        tutoGunText?.gameObject.SetActive(false); // Hide TutoGunText at the start
        tutoTimeText?.gameObject.SetActive(false); // Hide TutoTimeText at the start
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
        
        // Show tutorial texts one by one with sound and animation
        yield return ShowTutorialText(tutoGunText);
        yield return ShowTutorialText(tutoTimeText);

        scoreText?.gameObject.SetActive(true);
        levelText?.gameObject.SetActive(true);
        achievementText?.gameObject.SetActive(true);
        currentState = GameState.Playing;
        UpdateLevelUI();
    }

    IEnumerator ShowTutorialText(Text tutorialText)
    {
        if (tutorialText != null)
        {
            CanvasGroup canvasGroup = tutorialText.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = tutorialText.gameObject.AddComponent<CanvasGroup>();
            }

            tutorialText.gameObject.SetActive(true);
            if (tutoAudioSource != null)
            {
                tutoAudioSource.Play();
            }

            // Fade in
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }

            yield return new WaitForSeconds(5f); // Display for 5 seconds

            // Fade out
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }

            tutorialText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        // Ensure the tutorial texts follow the camera
        FollowCamera(tutoGunText);
        FollowCamera(tutoTimeText);

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

    void FollowCamera(Text tutorialText)
    {
        if (tutorialText != null && cameraTransform != null)
        {
            tutorialText.transform.position = cameraTransform.position + cameraTransform.forward * 2;
            tutorialText.transform.LookAt(cameraTransform);
            tutorialText.transform.Rotate(0, 180, 0);
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
