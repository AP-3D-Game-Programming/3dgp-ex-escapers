using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    private static GameState currentState = GameState.MainMenu;
    private static int currentLevel;

    public static GameState CurrentState => currentState;
    public static bool IsPaused => currentState == GameState.Paused;
    public static int CurrentLevel => currentLevel;

    /// <summary>
    /// Change the current game state
    /// </summary>
    public static void SetGameState(GameState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        EventManager.TriggerGameStateChanged(newState);
        
        switch (newState)
        {
            case GameState.Paused:
                Time.timeScale = 0f;
                EventManager.TriggerGamePaused();
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                EventManager.TriggerGameResumed();
                break;
            case GameState.GameOver:
                EventManager.TriggerGameOver();
                break;
        }
    }

    public static void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
    }

    public static void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }
    
    public static void StartGame()
    {
        currentLevel = 1;
        SetGameState(GameState.Playing);
        EventManager.TriggerGameStarted();
    }
    
    public static void ResetGame()
    {
        currentLevel = 1;
        SetGameState(GameState.MainMenu);
    }

    public static void CompleteLevel()
    {
        EventManager.TriggerLevelCompleted(currentLevel);
        currentLevel++;
    }
}
