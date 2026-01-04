using System;
using System.Collections.Generic;
using UnityEngine;

public static class OnClickManager
{
    private static bool clicksEnabled = true;
    private static Dictionary<string, Action> buttonActions = new Dictionary<string, Action>();
    
    public static bool ClicksEnabled
    {
        get => clicksEnabled;
        set => clicksEnabled = value;
    }
    
    static OnClickManager()
    {
        RegisterButtonAction("Play", () => { }); 
        RegisterButtonAction("Pause", () => GameManager.PauseGame());
        RegisterButtonAction("Resume", () => GameManager.ResumeGame());
        RegisterButtonAction("Restart", () => SceneManager.LoadScene("Main scene")); // this one is optional, so can be removed if it stays unused
        RegisterButtonAction("MainMenu", () => SceneManager.LoadScene("MainMenu"));
        RegisterButtonAction("Quit", () => QuitGame());
    }
    
    /// <summary>
    /// Register a custom action for a button ID
    /// </summary>
    public static void RegisterButtonAction(string buttonId, Action action)
    {
        if (buttonActions.ContainsKey(buttonId))
        {
            buttonActions[buttonId] = action;
        }
        else
        {
            buttonActions.Add(buttonId, action);
        }
    }
    
    /// <summary>
    /// Unregister a button action
    /// </summary>
    public static void UnregisterButtonAction(string buttonId)
    {
        if (buttonActions.ContainsKey(buttonId))
        {
            buttonActions.Remove(buttonId);
        }
    }
    
    /// <summary>
    /// Register a button click with an identifier
    /// </summary>
    public static void RegisterClick(string buttonId)
    {
        if (!clicksEnabled) return;
        
        Debug.Log($"Button clicked: {buttonId}");
        EventManager.TriggerButtonClicked(buttonId);
        
        if (buttonActions.ContainsKey(buttonId))
        {
            buttonActions[buttonId]?.Invoke();
        }
    }
    
    /// <summary>
    /// Enable or disable all clicks
    /// </summary>
    public static void SetClicksEnabled(bool enabled)
    {
        clicksEnabled = enabled;
    }
    
    private static void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}