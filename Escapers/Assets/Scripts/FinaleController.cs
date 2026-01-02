using UnityEngine;

public class FinaleController : MonoBehaviour
{
    [Header("Finale Objects")]
    public GameObject stone;              // De steen die moet verdwijnen
    public GameObject magicHoldEffect;    // Particle/magic die de steen tegenhoudt
    public FinalRuneManager runeManager;  // Finale RuneManager
    public GameObject monster;            // Monster dat verpletterd wordt

    [Header("Sound")]
    public AudioClip impactSound;         // Geluid wanneer steen verdwijnt

    [Header("Timer")]
    public FinalRoomTimer finalTimer;     // De 2-minuten timer

    private bool ropeCutTriggered = false;
    private bool stoneReleased = false;

    private void Start()
    {
        // Magic uit bij start
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(false);
    }

    // Wordt aangeroepen door FinalRopeCut
    public void OnRopeCut()
    {
        ropeCutTriggered = true;

        // Magic aan
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(true);

        // Finale runes tonen
        if (runeManager != null)
            runeManager.RevealRunes();
    }

    // Wordt aangeroepen door FinalRuneManager wanneer alle runes kapot zijn
    public void OnAllRunesDestroyed()
    {
        if (stoneReleased)
            return;

        stoneReleased = true;

        // Magic uit
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(false);

        // Impact sound
        if (impactSound != null)
            AudioSource.PlayClipAtPoint(impactSound, stone.transform.position);

        // Steen verwijderen
        if (stone != null)
            Destroy(stone);

        // Monster vernietigen
        if (monster != null)
            Destroy(monster);

        // Timer stoppen
        if (finalTimer != null)
            finalTimer.StopTimer();
    }
}