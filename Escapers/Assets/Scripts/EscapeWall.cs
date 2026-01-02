using UnityEngine;

public class EscapeWall : MonoBehaviour
{
    [Header("UI")]
    public GameObject escapeMessageUI;

    [Header("Settings")]
    public float delayBeforeMenu = 3f;
    public string menuSceneName = "MainMenu";

    private bool triggered = false;

    private void Start()
    {
        if (escapeMessageUI != null)
            escapeMessageUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (escapeMessageUI != null)
                escapeMessageUI.SetActive(true);

            Time.timeScale = 0f;

            StartCoroutine(EscapeSequence());
        }
    }

    private System.Collections.IEnumerator EscapeSequence()
    {
        yield return new WaitForSecondsRealtime(delayBeforeMenu);

        Time.timeScale = 1f;

        SceneManager.LoadSceneInstant(menuSceneName);
    }
}