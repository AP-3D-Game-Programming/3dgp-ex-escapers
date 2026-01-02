using UnityEngine;
using TMPro;

public class RuneManager : MonoBehaviour
{
    public GameObject[] runes;          // Sleep hier alle rune prefabs in
    public GameObject missingText;      // UI: "Runes missing: X"
    public GameObject magicalBarrier;   // De deur/barrier die moet verdwijnen

    private int runesLeft;

    // finale mode
    public bool finaleActive = false;

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
        // Finale is al bezig → runes NIET opnieuw tonen
        if (finaleActive)
        {
            Debug.Log("RuneManager: Finale is actief → runes worden niet opnieuw getoond");
            return;
        }

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
            Debug.Log("Alle runes vernietigd — barrier verdwijnt!");

            if (magicalBarrier != null)
                magicalBarrier.SetActive(false);
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