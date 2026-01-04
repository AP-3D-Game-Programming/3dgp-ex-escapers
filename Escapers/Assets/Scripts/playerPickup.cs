using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Collider nearbyPickup;

    public GameObject daggerInHand;

    // UI tekst
    public TMPro.TextMeshProUGUI pickupText;
    public AudioSource pickupaudio;


    private void Start()
    {
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            nearbyPickup = other;

            // UI tonen
            if (pickupText != null)
            {
                pickupText.text = $"[E] Pick up {other.name}";
                pickupText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp") && nearbyPickup == other)
        {
            nearbyPickup = null;

            // UI verbergen
            if (pickupText != null)
                pickupText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (nearbyPickup == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickupaudio != null)
                pickupaudio.Play();

            // FIX: clone verwijderen uit de naam
            string itemName = nearbyPickup.name.Replace("(Clone)", "").Trim();

            // UI verbergen
            if (pickupText != null)
                pickupText.gameObject.SetActive(false);

            // Inventory toevoegen
            InventoryManager.Instance.AddItem(itemName);

            // Dagger special case
            if (itemName.ToLower().Contains("dagger"))
            {
                if (daggerInHand != null)
                    daggerInHand.SetActive(true);
                EventManager.TriggerDaggerPickUp();
            }

            Destroy(nearbyPickup.gameObject);
            nearbyPickup = null;
        }
    }
}