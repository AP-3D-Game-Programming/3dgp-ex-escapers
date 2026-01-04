using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource), typeof(TMP_Text))]
public class PlayerSpeechManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Subtitles")]
    [SerializeField] private TMP_Text subtitleText;
    
    [Header("Startup Speech")]
    [SerializeField] private SpeechData spawnSpeech;
    [SerializeField] private SpeechData secondPuzzleSpeech;
    [SerializeField] private SpeechData altarStartSpeech;
    [SerializeField] private SpeechData thirdPuzzleSpeech;
    [SerializeField] private SpeechData finalCutCompletedSpeech;
    [SerializeField] private SpeechData finalCompletedSpeech;

    private Coroutine subtitleRoutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);
    }

    void Start()
    {
        PlaySpeech(spawnSpeech);
        EventManager.OnDaggerPickUp += PlaySecondPuzzleSpeech;
        EventManager.OnAltarStarted += PlayAltarStartSpeech;
        EventManager.OnAltarCompleted += PlayThirdPuzzleSpeech;
        EventManager.OnFinalCutCompleted += PlayFinalCutSpeech;
        EventManager.OnFinalCompleted += PlayFinalCompleted;
    }

    void OnDestroy()
    {
        EventManager.OnDaggerPickUp -= PlaySecondPuzzleSpeech;
        EventManager.OnAltarStarted -= PlayAltarStartSpeech;
        EventManager.OnAltarCompleted -= PlayThirdPuzzleSpeech;
        EventManager.OnFinalCutCompleted -= PlayFinalCutSpeech;
        EventManager.OnFinalCompleted -= PlayFinalCompleted;        
    }

    private void PlaySecondPuzzleSpeech() => PlaySpeech(secondPuzzleSpeech);
    private void PlayAltarStartSpeech() => PlaySpeech(altarStartSpeech);
    private void PlayThirdPuzzleSpeech() => PlaySpeech(thirdPuzzleSpeech);
    private void PlayFinalCutSpeech() => PlaySpeech(finalCutCompletedSpeech);
    private void PlayFinalCompleted() => PlaySpeech(finalCompletedSpeech);

    public void PlaySpeech(SpeechData speech)
    {
        if (speech == null) return;
        if (speech.hasPlayed) return;

        speech.hasPlayed = true;

        float duration = 0f;

        if (speech.clip != null)
        {
            audioSource.PlayOneShot(speech.clip);
            duration = speech.clip.length;
        }

        if (!string.IsNullOrEmpty(speech.text) && subtitleText != null)
        {
            if (subtitleRoutine != null)
                StopCoroutine(subtitleRoutine);

            subtitleRoutine = StartCoroutine(ShowSubtitle(speech.text, duration));
        }
    }

    private IEnumerator ShowSubtitle(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.gameObject.SetActive(true);

        if (duration <= 0f)
            duration = 5f;

        yield return new WaitForSeconds(duration);

        subtitleText.gameObject.SetActive(false);
        subtitleRoutine = null;
    }
}

[System.Serializable]
public class SpeechData
{
    public AudioClip clip;
    [TextArea]
    public string text;

    [HideInInspector]
    public bool hasPlayed;
}
