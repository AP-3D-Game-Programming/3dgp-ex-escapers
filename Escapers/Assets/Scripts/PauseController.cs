using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pausePanel;
    
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private bool isPaused;
    
    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        
        HidePauseMenu();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        ShowPauseMenu();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnClickManager.RegisterClick("Pause");
    }

    private void ResumeGame()
    {
        isPaused = false;
        HidePauseMenu();

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        OnClickManager.RegisterClick("Resume");        
    }

    private void OnResumeClicked()
    {
        HidePauseMenu();
    }
    
    private void OnRestartClicked()
    {
        OnClickManager.RegisterClick("Restart");
    }
    
    private void OnQuitClicked()
    {
        OnClickManager.RegisterClick("MainMenu");
    }
    
    private void ShowPauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }
    
    private void HidePauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
    
    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        restartButton.onClick.RemoveListener(OnRestartClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
    }
}
