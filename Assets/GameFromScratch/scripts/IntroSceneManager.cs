using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public string mainGameSceneName = "MainGameScene"; // Name of the main game scene
    public float introDuration = 7f; // Duration to play the intro scene (in seconds)

    void Start()
    {
        StartCoroutine(PlayIntroAndLoadMainGame());
    }

    IEnumerator PlayIntroAndLoadMainGame()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(introDuration);

        // Load the main game scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainGameSceneName, LoadSceneMode.Single);

        // Wait until the main game scene has finished loading
        yield return new WaitUntil(() => asyncLoad.isDone);

        Debug.Log("IntroSceneManager: Main game scene loaded.");
    }
}
