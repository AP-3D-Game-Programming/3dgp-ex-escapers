using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleInformationManager : MonoBehaviour
{
    [SerializeField] private TMP_Text puzzleInformationText;

    [Header("Startup Puzzle Information")]
    [SerializeField] private PuzzleData spawnInformation;
    [SerializeField] private PuzzleData secondPuzzleInformation;
    [SerializeField] private PuzzleData altarStartInformation;
    [SerializeField] private PuzzleData thirdPuzzleInformation;

    private PuzzleData currentData;

    private void Awake()
    {
        if (puzzleInformationText == null)
            puzzleInformationText = GetComponent<TMP_Text>();

        puzzleInformationText.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(spawnCoroutine());
        EventManager.OnDaggerPickUp += ShowSecondPuzzleInformation;
        EventManager.OnAltarStarted += ShowAltarStartInformation;
        EventManager.OnAltarCompleted += ShowThirdPuzzleInformation;
    }

    void OnDestroy()
    {
        EventManager.OnDaggerPickUp -= ShowSecondPuzzleInformation;
        EventManager.OnAltarStarted -= ShowAltarStartInformation;
        EventManager.OnAltarCompleted -= ShowThirdPuzzleInformation;
    }

    private void ShowSecondPuzzleInformation() => ShowPuzzleInformation(secondPuzzleInformation);
    private void ShowAltarStartInformation() => ShowPuzzleInformation(altarStartInformation);
    private void ShowThirdPuzzleInformation() => ShowPuzzleInformation(thirdPuzzleInformation);

    private IEnumerator spawnCoroutine()
    {
        yield return new WaitForSeconds(5f);
        ShowPuzzleInformation(spawnInformation);
    }

    public void ShowPuzzleInformation(PuzzleData data)
    {
        if (data == null) return;
        if (data.hasPlayed) return;

        if (currentData != null && !currentData.hasPlayed)
        {
            currentData.hasPlayed = true;
        }

        currentData = data;

        puzzleInformationText.text = data.text;
        puzzleInformationText.gameObject.SetActive(true);
    }

    public void ClearCurrentInformation()
    {
        if (currentData != null)
        {
            currentData.hasPlayed = true;
            currentData = null;
        }

        puzzleInformationText.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class PuzzleData
{
    [TextArea]
    public string text;

    [HideInInspector]
    public bool hasPlayed;
}