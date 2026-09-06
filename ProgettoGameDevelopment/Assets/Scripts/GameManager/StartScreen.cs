using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public static bool isGameStarted = false;
    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            isGameStarted = true;
        }
        else
        {
            isGameStarted = false; // tutte le volte che avviene un GameOver o il giocatore preme il pulsante quit del menu giocoIniziato deve essere impostato a false perché altrimenti dopo aver quittato il gioco dal menu e aver premuto il tasto di apertura menu come primo tasto per avviare la partita il menu viene aperto ma il gioco parte lo stesso
        }
    }

    void Start()
    {
        if (isGameStarted)
        {
            Time.timeScale = 1f;

            healthBar.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;

            healthBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            isGameStarted = true;
            healthBar.gameObject.SetActive(true);
            Time.timeScale = 1f;

            gameObject.SetActive(false);
        }
    }
}