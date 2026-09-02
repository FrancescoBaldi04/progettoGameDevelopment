using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public static bool giocoIniziato = false;
    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
              giocoIniziato = true;
        }
        else
        {
              giocoIniziato = false; // tutte le volte che avviene un GameOver o il giocatore preme il pulsante quit del menu giocoIniziato deve essere impostato a false perché altrimenti dopo aver quittato il gioco dal menu e aver premuto il tasto di apertura menu come primo tasto per avviare la partita il menu viene aperto ma il gioco parte lo stesso
        }
    }

    void Start()
    {
        if (giocoIniziato)
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
            giocoIniziato = true;
            healthBar.gameObject.SetActive(true);
            Time.timeScale = 1f;

            gameObject.SetActive(false);
        }
    }
}