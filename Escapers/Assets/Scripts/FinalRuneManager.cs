using UnityEngine;
using TMPro;

public class FinalRuneManager : MonoBehaviour
{
    [Header("Finale Runes")]
    public GameObject[] runes;          // Finale runes
    public GameObject missingText;      // UI: "Runes missing: X"

    private int runesLeft;

    [Header("Finale Logic")]
    public FinaleController finaleController; // Finale controller koppelen

    private void Start()
    {
        // Alle runes verbergen bij start
        foreach (var rune in runes)
            rune.SetActive(false);

        runesLeft = runes.Length;

        UpdateMissingText();
    }

    public void RevealRunes()
    {
        foreach (var rune in runes)
            rune.SetActive(true);

        if (missingText != null)
            missingText.SetActive(true);

        UpdateMissingText();
    }

    public void RuneDestroyed()
    {
        runesLeft--;

        UpdateMissingText();

        if (runesLeft <= 0)
        {
            Debug.Log("Finale: Alle runes vernietigd!");

            // FinaleController laten weten dat runes klaar zijn
            if (finaleController != null)
                finaleController.OnAllRunesDestroyed();
        }
    }

    private void UpdateMissingText()
    {
        if (missingText != null)
        {
            TextMeshProUGUI tmp = missingText.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = "Runes missing: " + runesLeft;
        }
    }

    public bool AllRunesDestroyed()
    {
        return runesLeft <= 0;
    }
}