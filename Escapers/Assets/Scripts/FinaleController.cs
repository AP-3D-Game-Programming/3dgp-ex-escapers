using UnityEngine;

public class FinaleController : MonoBehaviour
{
    [Header("Finale Objects")]
    public GameObject stone;              // De steen die moet vallen
    public GameObject magicHoldEffect;    // Particle/magic die de steen tegenhoudt
    public RuneManager runeManager;       // Finale RuneManager
    public RopeCut ropeCut;               // De rope die je moet cutten
    public GameObject monster;            // Monster dat verpletterd wordt

    [Header("Timer")]
    public FinalRoomTimer finalTimer;     // De 2-minuten timer

    private bool ropeCutTriggered = false;
    private bool stoneReleased = false;

    private void Start()
    {
        // Magic uit bij start
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(false);

        // Steen mag niet vallen tot runes klaar zijn
        Rigidbody rb = stone.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    private void Update()
    {
        // Rope is gesneden maar steen nog niet vrijgegeven
        if (ropeCutTriggered && !stoneReleased)
        {
            // Check of runes vernietigd zijn
            if (runeManager.AllRunesDestroyed())
            {
                ReleaseStone();
            }
        }
    }

    public void OnRopeCut()
    {
        ropeCutTriggered = true;

        // Activeer magic hold
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(true);

        // Finale mode activeren in RuneManager
        runeManager.finaleActive = true;

        // Laat runes verschijnen
        runeManager.RevealRunes();
    }

    private void ReleaseStone()
    {
        stoneReleased = true;

        // Magic weg
        if (magicHoldEffect != null)
            magicHoldEffect.SetActive(false);

        // Steen laten vallen
        Rigidbody rb = stone.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        // Monster vernietigen
        if (monster != null)
            Destroy(monster);

        // Timer stoppen
        if (finalTimer != null)
            finalTimer.StopTimer();
    }
}