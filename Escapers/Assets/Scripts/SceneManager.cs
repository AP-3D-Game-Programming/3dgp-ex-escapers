using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static string currentScene;
    
    static SceneManager()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
    }
    
    public static string CurrentScene => currentScene;

    /// <summary>
    /// Load a scene by name
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        EventManager.TriggerSceneLoading(sceneName);
        LoadingScreenManager.LoadSceneWithLoadingScreen(sceneName);
    }

    /// <summary>
    /// Load a scene by build index WITH loading screen
    /// </summary>
    public static void LoadScene(int sceneIndex)
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(sceneIndex).name;
        EventManager.TriggerSceneLoading(sceneName);
        LoadingScreenManager.LoadSceneWithLoadingScreen(sceneIndex);
    }

    /// <summary>
    /// Load a scene WITHOUT loading screen (instant)
    /// </summary>
    public static void LoadSceneInstant(string sceneName)
    {
        EventManager.TriggerSceneLoading(sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Reload the current scene WITH loading screen
    /// </summary>
    public static void ReloadCurrentScene()
    {
        Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        LoadScene(activeScene.name);
    }
    
    /// <summary>
    /// Get the active scene name
    /// </summary>
    public static string GetActiveSceneName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
    
    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        EventManager.TriggerSceneLoaded(scene.name);
    }
}
