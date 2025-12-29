using UnityEngine;

public class RuneDestroy : MonoBehaviour
{
    public AudioSource runeSound;
    public GameObject interactText;

    private bool playerInside = false;

    private void Start()
    {
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
            // Speel destroy sound
            if (runeSound != null)
                AudioSource.PlayClipAtPoint(runeSound.clip, transform.position, 1f);

            // UI verbergen
            if (interactText != null)
                interactText.SetActive(false);

            // Meld aan RuneManager dat deze rune weg is
            FindObjectOfType<RuneManager>().RuneDestroyed();

            // Rune verwijderen
            Destroy(gameObject);
        }
    }
}