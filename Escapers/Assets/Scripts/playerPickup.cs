using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Collider nearbyPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("In de buurt van: " + other.name);
            nearbyPickup = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup") && nearbyPickup == other)
        {
            Debug.Log("Niet meer in de buurt van: " + other.name);
            nearbyPickup = null;
        }
    }

    void Update()
    {
        if (nearbyPickup != null && Input.GetKeyDown(KeyCode.E))
        {
            string itemName = nearbyPickup.name;

            // Voeg ALTIJD toe aan de inventory lijst
            InventoryManager.Instance.AddItem(itemName);

            // Verwijder het object uit de wereld
            Destroy(nearbyPickup.gameObject);

            nearbyPickup = null;
        }
    }
}