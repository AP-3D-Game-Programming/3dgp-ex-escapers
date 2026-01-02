using UnityEngine;
using TMPro;

public class FinalRoomTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 120f; // 2 minuten
    private float timeRemaining;
    private bool timerRunning = false;

    [Header("UI")]
    public GameObject timerUI;
    private TextMeshProUGUI timerText;

    private LivesManager livesManager;

    private void Start()
    {
        livesManager = FindObjectOfType<LivesManager>();

        timeRemaining = startTime;

        if (timerUI != null)
        {
            timerText = timerUI.GetComponent<TextMeshProUGUI>();
            timerUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Timer resetten bij binnenkomen
            timeRemaining = startTime;
            timerRunning = true;

            if (timerUI != null)
            {
                timerUI.SetActive(true);
                timerText.text = "02:00"; // UI reset
            }
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