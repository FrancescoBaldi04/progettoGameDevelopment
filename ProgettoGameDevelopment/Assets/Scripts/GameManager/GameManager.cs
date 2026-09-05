using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    [SerializeField] private float timeBeforeRestart = 1.2f;

    // =========================
    // POTENZIAMENTI
    // =========================

    public bool hasTrojanHorse { get; private set; }
    public bool hasZipBomb { get; private set; }
    public bool hasWorm { get; private set; }


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

    public void BossDefeated()
    {
        Time.timeScale = 0f;
        PauseManager.pauseManager.ShowVictoryScreen();
    }

    // =========================
    // SBLOCCO POTENZIAMENTI
    // =========================

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


    // =========================
    // GAME OVER
    // =========================

    public void GameOver()
    {
        StartCoroutine(restartLevelRoutine());
    }


    private IEnumerator restartLevelRoutine()
    {
        yield return new WaitForSeconds(timeBeforeRestart);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        hasTrojanHorse = false;
        hasZipBomb = false;
        hasWorm = false;

        SceneManager.LoadScene(0);
    }
}