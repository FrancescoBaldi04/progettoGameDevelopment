using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; 
using TMPro;

public class PauseManager : MonoBehaviour
{
    public static PauseManager pauseManager { get; private set;} // Pattern Singleton

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject description;
    
    [SerializeField] private GameObject resumeButton; // Riferimento al bottone "Resume" per selezionarlo in automatico
     
    [SerializeField] private TextMeshProUGUI wormText; // Riferimenti ai tre oggetti relativi al testo dei power
    [SerializeField] private string wormUIdescription;
    [SerializeField] private TextMeshProUGUI trojanHorseText;
    [SerializeField] private string trojanHorseUIdescription;
    [SerializeField] private TextMeshProUGUI zipBombText;
    [SerializeField] private string zipBombUIdescription;

    private bool isPaused = false;
    private bool showingCommands = false;

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

    private void OnDestroy()
    {
        if (pauseManager == this)
        {
            pauseManager = null;
        }        
    }

    private void Start()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
        pauseMenuPanel.SetActive(false);
        description.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.gameManager.hasWorm)
        {
            UpdateWormText();
        }

        if (GameManager.gameManager.hasZipBomb)
        {
            UpdateZipBombText();
        }

        if (GameManager.gameManager.hasTrojanHorse)
        {
            UpdateTrojanHorsetext();
        }
        
        if(showingCommands ){
            if(Keyboard.current.bKey.wasPressedThisFrame){
                description.SetActive(false);
                pauseMenuPanel.SetActive(true);
                showingCommands = false;
                if (resumeButton != null && EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(resumeButton);
                }
            }
            return;
        }

        if (StartScreen.isGameStarted && Keyboard.current != null &&
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

    private void OnEnable()
    {
        GameManager.OnBossDefeated += ShowVictoryScreen;        
    }

    private void OnDisable()
    {
        GameManager.OnBossDefeated -= ShowVictoryScreen;
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
    public void Controls()
    {
        pauseMenuPanel.SetActive(false);
        description.SetActive(true);
        showingCommands = true;
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

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    public void UpdateWormText() 
    { 
        if (wormText != null) wormText.text = wormUIdescription; 
    }
    
    public void UpdateTrojanHorsetext() 
    { 
        if (trojanHorseText != null) trojanHorseText.text = trojanHorseUIdescription; 
    }
    
    public void UpdateZipBombText() 
    { 
        if (zipBombText != null) zipBombText.text = zipBombUIdescription; 
    }
}
