using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; 
using TMPro;
using UnityEngine.InputSystem.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager pauseManager { get; private set;} // Singleton

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject description;
    
    [SerializeField] private GameObject resumeButton; 
     
    [SerializeField] private TextMeshProUGUI wormText; 
    [SerializeField] private string wormUIdescription;
    [SerializeField] private TextMeshProUGUI trojanHorseText;
    [SerializeField] private string trojanHorseUIdescription;
    [SerializeField] private TextMeshProUGUI zipBombText;
    [SerializeField] private string zipBombUIdescription;

    private InputSystemUIInputModule uiInputModule;

    private bool isPaused = false;
    private bool showingCommands = false;

    private void Awake()
    {
        if (pauseManager == null) // Singleton pattern
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
        if (victoryScreen != null) // Initialize UI panels state
        {
            victoryScreen.SetActive(false);
        }
        pauseMenuPanel.SetActive(false);
        description.SetActive(false);
        uiInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
    }

    private void Update()
    {   
        // Handle back navigation from the controls screen
        if(showingCommands ){ 
            if(Keyboard.current.bKey.wasPressedThisFrame){
                description.SetActive(false);
                pauseMenuPanel.SetActive(true);
                showingCommands = false;
                if (resumeButton != null && EventSystem.current != null) // Restore navigation focus to the Resume button
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(resumeButton);
                }
            }
            return;
        }

        // Toggle Pause menu
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
        Time.timeScale = 0f; // Freeze game physics and animations 
        isPaused = true;

        UpdatePowerUpTexts();
        DisableMouseInput();


        // Automatically highlight the default UI button for keyboard navigation 
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
        Time.timeScale = 1f; // Restore game time 
        isPaused = false;
        EnableMouseInput();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale before returning to main menu

        if (GameManager.gameManager != null)
        {
            GameManager.gameManager.ReturnToMainMenu();
        }
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void UpdatePowerUpTexts()
    {
        // Refresh unlocked power-up descriptions
        if (GameManager.gameManager == null) return;
        if (GameManager.gameManager.hasWorm) UpdateWormText();
        if (GameManager.gameManager.hasZipBomb)  UpdateZipBombText();
        if (GameManager.gameManager.hasTrojanHorse) UpdateTrojanHorseText();
    }

    public void UpdateWormText() 
    { 
        if (wormText != null) wormText.text = wormUIdescription; 
    }
    
    public void UpdateTrojanHorseText() 
    { 
        if (trojanHorseText != null) trojanHorseText.text = trojanHorseUIdescription; 
    }
    
    public void UpdateZipBombText() 
    { 
        if (zipBombText != null) zipBombText.text = zipBombUIdescription; 
    }
    private void DisableMouseInput()
{
    if (uiInputModule == null)
        return;

    uiInputModule.point.action.Disable();
    uiInputModule.leftClick.action.Disable();
    uiInputModule.rightClick.action.Disable();
    uiInputModule.middleClick.action.Disable();
}
private void EnableMouseInput()
{
    if (uiInputModule == null)
        return;

    uiInputModule.point.action.Enable();
    uiInputModule.leftClick.action.Enable();
    uiInputModule.rightClick.action.Enable();
    uiInputModule.middleClick.action.Enable();
}
}
