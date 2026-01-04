using UnityEngine;

public class SpeechTrigger : MonoBehaviour
{
    public SpeechData speech;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerSpeechManager speechManager =
            other.GetComponent<PlayerSpeechManager>();

        if (speechManager != null)
        {
            speechManager.PlaySpeech(speech);
        }
    }
}