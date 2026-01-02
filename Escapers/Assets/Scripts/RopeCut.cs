using UnityEngine;

public class RopeCut : MonoBehaviour
{
    [Header("Objects")]
    public GameObject chandelier;     
    public GameObject relicPrefab;    

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

        // Debug checks
        if (breakSound == null)
            Debug.Log("RopeCut: breakSound is NIET toegewezen");

        if (chandelier == null)
            Debug.Log("RopeCut: chandelier is NIET toegewezen");

        if (relicPrefab == null)
            Debug.Log("RopeCut: relicPrefab is NIET toegewezen");
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

        // Spawn relic op vaste positie
        Vector3 spawnPos = new Vector3(-5.9514f, 9.993f, -8.7396f);

        if (relicPrefab != null)
            Instantiate(relicPrefab, spawnPos, Quaternion.identity);

        // Chandelier verwijderen
        if (chandelier != null)
            Destroy(chandelier);

        // UI verbergen
        if (interactText != null)
            interactText.SetActive(false);

        // Rope verwijderen
        Destroy(gameObject);
    }
}