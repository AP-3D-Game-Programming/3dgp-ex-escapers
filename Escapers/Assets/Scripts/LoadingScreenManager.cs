using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Screen UI")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    
    [Header("Settings")]
    [SerializeField] private float minimumLoadTime = 1f;
    
    private static LoadingScreenManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (loadingScreenPanel != null)
                loadingScreenPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Load a scene with loading screen
    /// </summary>
    public static void LoadSceneWithLoadingScreen(string sceneName)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError("LoadingScreenManager instance not found!");
            SceneManager.LoadScene(sceneName);
        }
    }
    
    /// <summary>
    /// Load a scene with loading screen by index
    /// </summary>
    public static void LoadSceneWithLoadingScreen(int sceneIndex)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.LoadSceneAsync(sceneIndex));
        }
        else
        {
            Debug.LogError("LoadingScreenManager instance not found!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(true);
        
        float startTime = Time.realtimeSinceStartup;
        
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            UpdateLoadingUI(progress);
            
            if (asyncLoad.progress >= 0.9f)
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                if (elapsedTime < minimumLoadTime)
                {
                    yield return new WaitForSecondsRealtime(minimumLoadTime - elapsedTime);
                }
                
                UpdateLoadingUI(1f);
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(false);
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(true);
        
        float startTime = Time.realtimeSinceStartup;
        
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            UpdateLoadingUI(progress);
            
            if (asyncLoad.progress >= 0.9f)
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                if (elapsedTime < minimumLoadTime)
                {
                    yield return new WaitForSecondsRealtime(minimumLoadTime - elapsedTime);
                }
                
                UpdateLoadingUI(1f);
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        if (loadingScreenPanel != null)
            loadingScreenPanel.SetActive(false);
    }
    
    private void UpdateLoadingUI(float progress)
    {        
        if (loadingText != null)
        {
            if (progress < 0.33f)
                loadingText.text = "Loading assets...";
            else if (progress < 0.66f)
                loadingText.text = "Preparing environment...";
            else if (progress < 1f)
                loadingText.text = "Almost ready...";
            else
                loadingText.text = "Complete!";
        }
    }
}