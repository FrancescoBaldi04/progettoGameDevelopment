using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public static bool isGameStarted = false;
    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        // Automatically start the game if loading a scene other than "Main"
        if (SceneManager.GetActiveScene().name != "Main")
        {
            isGameStarted = true;
        }
        else
        {
            isGameStarted = false; 
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
            Time.timeScale = 0f; // Freeze gameplay and hide UI on the title screen
            healthBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Start gameplay when any key is pressed
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            isGameStarted = true;
            healthBar.gameObject.SetActive(true);
            Time.timeScale = 1f;

            gameObject.SetActive(false);
        }
    }
}