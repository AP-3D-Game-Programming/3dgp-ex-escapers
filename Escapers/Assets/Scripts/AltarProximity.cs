using UnityEngine;

public class AltarProximity : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;      // "Press E to speak"
    public GameObject giveItemsText;     // "Press X to give items"
    public GameObject instructionText;   // tekst die 10s zichtbaar blijft
    public GameObject completedText;     // tekst die 20s zichtbaar blijft

    [Header("Audio")]
    public AudioSource proximitySound;

    [Header("Altar Objects (set inactive in editor)")]
    public GameObject skullObject;
    public GameObject gemsObject;
    public GameObject statueObject;

    private bool playerInside = false;
    private bool instructionActive = false;

    private bool skullPlaced = false;
    private bool gemsPlaced = false;
    private bool statuePlaced = false;

    private void Start()
    {
        if (interactText != null) interactText.SetActive(false);
        if (giveItemsText != null) giveItemsText.SetActive(false);
        if (instructionText != null) instructionText.SetActive(false);
        if (completedText != null) completedText.SetActive(false);

        if (proximitySound == null)
            Debug.Log("AltarProximity: proximitySound is niet toegewezen");
    }

    private void Update()
    {
        if (!playerInside)
            return;

        // E → instructie tonen
        if (Input.GetKeyDown(KeyCode.E) && !instructionActive)
        {
            StartCoroutine(ShowInstruction());
        }

        // X → item plaatsen
        if (Input.GetKeyDown(KeyCode.X))
        {
            TryPlaceItem();
        }
    }

    private void TryPlaceItem()
    {
        Debug.Log("INVENTORY: " + string.Join(", ", InventoryManager.Instance.inventory));

        // SKULL
        if (!skullPlaced && InventoryManager.Instance.HasItem("skull"))
        {
            InventoryManager.Instance.RemoveItem("skull");
            skullObject.SetActive(true);
            skullPlaced = true;
            Debug.Log("Altar: Skull geplaatst");
            CheckAltarCompletion();
            return;
        }

        // GEMS
        if (!gemsPlaced && InventoryManager.Instance.HasItem("gems"))
        {
            InventoryManager.Instance.RemoveItem("gems");
            gemsObject.SetActive(true);
            gemsPlaced = true;
            Debug.Log("Altar: Gems geplaatst");
            CheckAltarCompletion();
            return;
        }

        // STATUE
        if (!statuePlaced && InventoryManager.Instance.HasItem("statue"))
        {
            InventoryManager.Instance.RemoveItem("statue");
            statueObject.SetActive(true);
            statuePlaced = true;
            Debug.Log("Altar: Statue geplaatst");
            CheckAltarCompletion();
            return;
        }

        Debug.Log("Altar: Geen geldig item in inventory");
    }

    private void CheckAltarCompletion()
    {
        if (skullPlaced && gemsPlaced && statuePlaced)
        {
            Debug.Log("Altar: COMPLEET!");

            // Stop geluid
            if (proximitySound != null && proximitySound.isPlaying)
                proximitySound.Stop();

            // UI verbergen
            if (interactText != null) interactText.SetActive(false);
            if (giveItemsText != null) giveItemsText.SetActive(false);

            // Interactie uitschakelen
            playerInside = false;

            // Finale tekst tonen
            StartCoroutine(ShowCompletedText());
        }
    }

    private System.Collections.IEnumerator ShowCompletedText()
    {
        if (completedText != null)
            completedText.SetActive(true);

        yield return new WaitForSeconds(20f);

        if (completedText != null)
            completedText.SetActive(false);

        Debug.Log("Altar: finale tekst verdwenen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (!instructionActive)
            {
                if (interactText != null) interactText.SetActive(true);
                if (giveItemsText != null) giveItemsText.SetActive(true);
            }

            if (proximitySound != null && !proximitySound.isPlaying)
                proximitySound.Play();

            Debug.Log("AltarProximity: speler is dichtbij het altaar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null) interactText.SetActive(false);
            if (giveItemsText != null) giveItemsText.SetActive(false);
            if (instructionText != null) instructionText.SetActive(false);

            if (proximitySound != null && proximitySound.isPlaying)
                proximitySound.Stop();

            Debug.Log("AltarProximity: speler is weg van het altaar");
        }
    }

    private System.Collections.IEnumerator ShowInstruction()
    {
        instructionActive = true;

        if (interactText != null) interactText.SetActive(false);
        if (giveItemsText != null) giveItemsText.SetActive(false);
        if (instructionText != null) instructionText.SetActive(true);

        Debug.Log("AltarProximity: instructie tekst getoond");

        yield return new WaitForSeconds(10f);

        if (instructionText != null) instructionText.SetActive(false);

        instructionActive = false;

        if (playerInside)
        {
            if (interactText != null) interactText.SetActive(true);
            if (giveItemsText != null) giveItemsText.SetActive(true);
        }

        Debug.Log("AltarProximity: instructie tekst verdwenen");
    }
}