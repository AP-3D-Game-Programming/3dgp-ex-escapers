using System;
using UnityEngine;

public static class EventManager
{
    // Scene events
    public static event Action<string> OnSceneLoading;
    public static event Action<string> OnSceneLoaded;

    // Game state events
    public static event Action<GameManager.GameState> OnGameStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    
    // Click Events
    public static event Action<string> OnButtonClicked;
    
    // Custom Game Events
    public static event Action OnPlayerDied;
    public static event Action<int> OnLevelCompleted;
    public static event Action OnDaggerPickUp;
    public static event Action OnAltarStarted;
    public static event Action OnAltarCompleted;

    // Trigger methods for events
    public static void TriggerSceneLoading(string sceneName) => OnSceneLoading?.Invoke(sceneName);
    public static void TriggerSceneLoaded(string sceneName) => OnSceneLoaded?.Invoke(sceneName);
    public static void TriggerGameStateChanged(GameManager.GameState state) => OnGameStateChanged?.Invoke(state);
    public static void TriggerGameStarted() => OnGameStarted?.Invoke();
    public static void TriggerGamePaused() => OnGamePaused?.Invoke();
    public static void TriggerGameResumed() => OnGameResumed?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerButtonClicked(string buttonId) => OnButtonClicked?.Invoke(buttonId);
    public static void TriggerPlayerDied() => OnPlayerDied?.Invoke();
    public static void TriggerLevelCompleted(int level) => OnLevelCompleted?.Invoke(level);
    public static void TriggerDaggerPickUp() => OnDaggerPickUp?.Invoke();
    public static void TriggerAltarStarted() => OnAltarStarted?.Invoke();
    public static void TriggerAltarCompleted() => OnAltarCompleted?.Invoke();
}
