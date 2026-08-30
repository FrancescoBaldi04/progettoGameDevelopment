using UnityEngine;
using UnityEngine.InputSystem;

public class StartScreen : MonoBehaviour
{
    public static bool giocoIniziato = false;

    void Awake()
    {
        giocoIniziato = false; // tutte le volte che avviene un GameOver o il giocatore preme il pulsante quit del menu giocoIniziato deve essere impostato a false perché altrimenti dopo aver quittato il gioco dal menu e aver premuto il tasto di apertura menu come primo tasto per avviare la partita il menu viene aperto ma il gioco parte lo stesso
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            giocoIniziato = true;

            Time.timeScale = 1f;

            gameObject.SetActive(false);
        }
    }
}