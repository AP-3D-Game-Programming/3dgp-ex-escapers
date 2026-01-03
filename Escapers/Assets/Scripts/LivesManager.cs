using UnityEngine;
using TMPro;

public class LivesManager : MonoBehaviour
{
    [Header("Lives Settings")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI")]
    public TextMeshProUGUI livesText;

    [Header("Respawn")]
    public Transform respawnPoint;   // beginpunt van de finale
    public GameObject fadeScreen;    // zwart scherm (Canvas Image)

    private void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();

        if (fadeScreen != null)
            fadeScreen.SetActive(false);
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            Debug.Log("Game Over — geen levens meer");
            // hier kan je een echte game over scene laden
            return;
        }

        StartCoroutine(RespawnRoutine());
    }

    public void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }

    public System.Collections.IEnumerator RespawnRoutine()
    {
        // Fade in
        if (fadeScreen != null)
        {
            fadeScreen.SetActive(true);
            CanvasGroup cg = fadeScreen.GetComponent<CanvasGroup>();

            cg.alpha = 0;
            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime * 1.5f;
                yield return null;
            }
        }

        // Teleport speler
        transform.position = respawnPoint.position;

        // 1 seconde wachten
        yield return new WaitForSeconds(1f);

        // Fade out
        if (fadeScreen != null)
        {
            CanvasGroup cg = fadeScreen.GetComponent<CanvasGroup>();

            while (cg.alpha > 0)
            {
                cg.alpha -= Time.deltaTime * 1.5f;
                yield return null;
            }

            fadeScreen.SetActive(false);
        }
    }
}