using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    
    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackFromSettings);
        }
        
        ShowMainMenu();
    }
    
    private void OnPlayClicked()
    {
        SceneManager.LoadScene("Main scene");
    }
    
    private void OnSettingsClicked()
    {
        OnClickManager.RegisterClick("Settings"); // TODO add this in OnClickManager
        ShowSettings();
    }
    
    private void OnQuitClicked()
    {
        OnClickManager.RegisterClick("Quit");
    }
    
    private void OnBackFromSettings()
    {
        ShowMainMenu();
    }
    
    private void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    private void ShowSettings()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
        
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackFromSettings);
        }
    }
}
