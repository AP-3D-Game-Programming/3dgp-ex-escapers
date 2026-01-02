using UnityEngine;

public class RopeCut : MonoBehaviour
{
    [Header("Settings")]
    public bool requiresDagger = true;

    [Header("Audio")]
    public AudioClip breakSound;

    [Header("UI")]
    public GameObject interactText;   // "[E] to cut rope"

    private bool playerInside = false;

    private void Start()
    {
        // UI verbergen bij start
        if (interactText != null)
            interactText.SetActive(false);

        if (breakSound == null)
            Debug.Log("RopeCut: breakSound is NIET toegewezen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check of speler een dagger heeft
            if (requiresDagger)
            {
                if (!InventoryManager.Instance.HasItem("Dagger"))
                {
                    Debug.Log("RopeCut: speler heeft geen dagger → kan rope niet snijden");
                    return;
                }
            }

            CutRope();
        }
    }

    private void CutRope()
    {
        // Speel geluid
        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        // UI verbergen
        if (interactText != null)
            interactText.SetActive(false);

        // FinaleController informeren dat rope is gesneden
        FinaleController finale = FindObjectOfType<FinaleController>();
        if (finale != null)
            finale.OnRopeCut();
        else
            Debug.LogError("FinaleController niet gevonden in de scene!");

        // Rope verwijderen
        Destroy(gameObject);
    }
}