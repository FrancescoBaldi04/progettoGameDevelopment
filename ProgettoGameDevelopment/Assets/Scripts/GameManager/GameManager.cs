using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    public static event Action OnBossDefeated;

    [SerializeField] private float timeBeforeRestart = 1.2f;

    public bool hasTrojanHorse { get; private set; }
    public bool hasZipBomb { get; private set; }
    public bool hasWorm { get; private set; }
    private bool startingTrojanHorse;
    private bool startingZipBomb;
    private bool startingWorm;


    private void Awake()
    {
        if (gameManager != null && gameManager != this) // pattern Singleton
        {
            Destroy(gameObject);
            return;
        }

        gameManager = this;

        // Mantiene il GameManager quando cambia scena
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SaveLevelPowerUps();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveLevelPowerUps();
        Time.timeScale = 1f;
    }
    private void SaveLevelPowerUps()
    {
        startingTrojanHorse = hasTrojanHorse;
        startingZipBomb = hasZipBomb;
        startingWorm = hasWorm;
    }
    private void RestoreLevelPowerUps()
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

    public void ReturnToMainMenu()
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