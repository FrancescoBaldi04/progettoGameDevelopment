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
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        gameManager = this;

        // Mantiene il GameManager quando cambia scena
        DontDestroyOnLoad(gameObject);
    }


    // =========================
    // SBLOCCO POTENZIAMENTI
    // =========================

    public void UnlockTrojanHorse()
    {
        hasTrojanHorse = true;

        Debug.Log("GameManager: Trojan Horse sbloccato!");
    }


    public void UnlockZipBomb()
    {
        hasZipBomb = true;

        Debug.Log("GameManager: Zip Bomb sbloccata!");
    }


    public void UnlockWorm()
    {
        hasWorm = true;

        Debug.Log("GameManager: Worm sbloccato!");
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

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}