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
                pickupText.text = $"Press E to pick up {other.name}";
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
            string itemName = nearbyPickup.name;

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
            }

            Destroy(nearbyPickup.gameObject);
            nearbyPickup = null;
        }
    }
}