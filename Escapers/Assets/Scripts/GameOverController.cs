using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button quitButton;
    
    private void Start()
    {
        replayButton.onClick.AddListener(OnReplayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        
        ShowGameOver();
    }
    
    private void OnReplayClicked()
    {
        OnClickManager.RegisterClick("Restart");
    }
    
    private void OnQuitClicked()
    {
        OnClickManager.RegisterClick("MainMenu");
    }
    
    private void OnBackFromSettings()
    {
        ShowGameOver();
    }
    
    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        replayButton.onClick.RemoveListener(OnReplayClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
    }
}
