using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager {get; private set;} 

    [SerializeField] private float timeBeforeRestart = 1.2f;

    // Pattern singleton in modo che esista solo un'istanza di questa classe e che essa fornisca un punto di accesso globale a quest'istanza
    void Awake(){
        if (gameManager == null){
            gameManager = this;
        }else{
            Destroy(gameObject); // per evitare duplicati se la scena viene ricaricata
        }
    }

    public void GameOver(){
        // inserire qui in futuro la UI per il GameOver, eventuale stop o modifica della musica e tutto ciò relativo all'animazione di morte
        StartCoroutine(restartLevelRoutine());
    }

    private IEnumerator restartLevelRoutine()
    {
        yield return new WaitForSeconds(timeBeforeRestart); // sospende la sua esecuzione per timeBeforeRestart secondi 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ricarico la scena
    }
}
