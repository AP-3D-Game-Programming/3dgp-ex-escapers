using UnityEngine;

public class FinalRopeCut : MonoBehaviour
{
    [Header("Settings")]
    public bool requiresDagger = true;

    [Header("UI")]
    public GameObject interactText;   // "[E] to cut rope"

    private bool playerInside = false;

    private void Start()
    {
        // UI verbergen bij start
        if (interactText != null)
            interactText.SetActive(false);
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
            if (requiresDagger && !InventoryManager.Instance.HasItem("Dagger"))
            {
                Debug.Log("FinalRopeCut: speler heeft geen dagger → kan rope niet snijden");
                return;
            }

            CutRope();
        }
    }

    private void CutRope()
    {
        // FinaleController triggeren
        FinaleController finale = FindObjectOfType<FinaleController>();
        if (finale != null)
            finale.OnRopeCut();
        else
            Debug.LogError("FinaleController niet gevonden in de scene!");

        // UI verbergen
        if (interactText != null)
            interactText.SetActive(false);

        // Rope verwijderen
        Destroy(gameObject);
    }
}