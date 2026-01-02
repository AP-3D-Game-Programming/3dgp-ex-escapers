using UnityEngine;
using TMPro;

public class FinalRoomTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 120f; // 2 minuten
    private bool timerRunning = false;

    [Header("UI")]
    public GameObject timerUI;          // Het UI object (TextMeshPro)
    private TextMeshProUGUI timerText;  // De TMP component

    private LivesManager livesManager;

    private void Start()
    {
        // LivesManager zoeken
        livesManager = FindObjectOfType<LivesManager>();

        if (timerUI != null)
        {
            timerText = timerUI.GetComponent<TextMeshProUGUI>();
            timerUI.SetActive(false); // timer is onzichtbaar tot speler binnenkomt
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !timerRunning)
        {
            timerRunning = true;

            if (timerUI != null)
                timerUI.SetActive(true);
        }
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            if (timerText != null)
                timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            timerRunning = false;
            timeRemaining = 0;

            if (timerText != null)
                timerText.text = "00:00";

            OnTimerFinished();
        }
    }

    private void OnTimerFinished()
    {
        Debug.Log("Finale timer is afgelopen!");

        // Leven verliezen + respawn
        if (livesManager != null)
            livesManager.LoseLife();
        else
            Debug.LogError("LivesManager niet gevonden in de scene!");
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}