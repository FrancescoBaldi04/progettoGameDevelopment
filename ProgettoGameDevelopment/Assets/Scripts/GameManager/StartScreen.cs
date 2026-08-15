using UnityEngine;
using UnityEngine.InputSystem;

public class StartScreen : MonoBehaviour
{
    public static bool giocoIniziato = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            giocoIniziato = true;
            gameObject.SetActive(false);
        }
    }
}