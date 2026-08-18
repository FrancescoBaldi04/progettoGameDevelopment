using UnityEngine;
using UnityEngine.InputSystem;

public class StartScreen : MonoBehaviour
{
    public static bool giocoIniziato = false;

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