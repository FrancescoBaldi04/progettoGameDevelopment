using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; } // Singleton

    public static event Action OnBossDefeated; // Event broadcast when the final boss is defeated

    [SerializeField] private float timeBeforeRestart = 1.2f;

    public bool hasTrojanHorse { get; private set; }
    public bool hasZipBomb { get; private set; }
    public bool hasWorm { get; private set; }
    private bool startingTrojanHorse;
    private bool startingZipBomb;
    private bool startingWorm;


    private void Awake()
    {
        if (gameManager != null && gameManager != this) // Singleton pattern
        {
            Destroy(gameObject);
            return;
        }

        gameManager = this;

        // Preserve GameManager across scene transitions 
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SaveLevelPowerUps();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        SaveLevelPowerUps(); // Save level checkpoint state and restore normal game time upon loading 
        Time.timeScale = 1f;
    }
    private void SaveLevelPowerUps() // Store state of power-ups at the start of the current level
    {
        startingTrojanHorse = hasTrojanHorse;
        startingZipBomb = hasZipBomb;
        startingWorm = hasWorm;
    }
    private void RestoreLevelPowerUps() // Revert power-ups to initial level state upon player death
    {
        hasTrojanHorse = startingTrojanHorse;
        hasZipBomb = startingZipBomb;
        hasWorm = startingWorm;
    }

    public void BossDefeated() 
    {
        Time.timeScale = 0f;
        OnBossDefeated?.Invoke();
    }

    public void ReturnToMainMenu() // Reset all power-ups progress and return to the main scene
    {
        hasTrojanHorse = false;
        hasZipBomb = false;
        hasWorm = false;
        startingTrojanHorse = false;
        startingZipBomb = false;
        startingWorm = false;

        SceneManager.LoadScene(0);
    }

    // Power-ups

    public void UnlockTrojanHorse()
    {
        hasTrojanHorse = true;
    }
    public void UnlockZipBomb()
    {
        hasZipBomb = true;
    }

    public void UnlockWorm()
    {
        hasWorm = true;
    }

    // Game Over
    public void GameOver()
    {
         RestoreLevelPowerUps();
        StartCoroutine(restartLevelRoutine());
    }

    private IEnumerator restartLevelRoutine()
    {
        yield return new WaitForSeconds(timeBeforeRestart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
     private void OnDestroy()
    {
        if (gameManager == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}