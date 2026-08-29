using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; 
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager pauseManager { get; private set;} // Pattern Singleton

    [SerializeField] private GameObject pauseMenuPanel;

    [SerializeField] private GameObject resumeButton; // Riferimento al bottone "Resume" per selezionarlo in automatico

    [SerializeField] private TextMeshProUGUI wormText; // Riferimenti ai tre oggetti relativi al testo dei power
    [SerializeField] private TextMeshProUGUI trojanHorseText;
    [SerializeField] private TextMeshProUGUI zipBombText;

    private bool isPaused = false;

    private void Awake()
    {
        if (pauseManager == null) 
        {
            pauseManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && 
            (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Seleziona automaticamente il bottone "Resume" quando metti in pausa
        if (resumeButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Pulisce selezioni precedenti
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        if (GameManager.gameManager != null)
        {
            GameManager.gameManager.ReturnToMainMenu();
        }
    }

    public void UpdateWormText(string newText) 
    { 
        if (wormText != null) wormText.text = newText; 
    }
    
    public void UpdateTrojanHorsetext(string newText) 
    { 
        if (trojanHorseText != null) trojanHorseText.text = newText; 
    }
    
    public void UpdateZipBombText(string newText) 
    { 
        if (zipBombText != null) zipBombText.text = newText; 
    }
}
