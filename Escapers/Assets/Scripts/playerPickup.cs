using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private Collider nearbyPickup;

    // Dagger die al onder de hand hangt maar onzichtbaar start
    public GameObject daggerInHand;

    private void Start()
    {
        Debug.Log("PlayerPickup START → Script actief op object: " + gameObject.name);

        if (daggerInHand == null)
            Debug.LogWarning("daggerInHand is NIET toegewezen in de Inspector!");
        else
            Debug.Log("daggerInHand is correct toegewezen: " + daggerInHand.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter → geraakt object: " + other.name + " | Tag: " + other.tag);

        if (other.CompareTag("PickUp"))
        {
            Debug.Log("Pickup gedetecteerd: " + other.name);
            nearbyPickup = other;
        }
        else
        {
            Debug.Log("Object heeft GEEN Pickup tag.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit → verlaten object: " + other.name);

        if (other.CompareTag("PickUp") && nearbyPickup == other)
        {
            Debug.Log("Pickup verlaten: " + other.name);
            nearbyPickup = null;
        }
    }

    void Update()
    {
        if (nearbyPickup == null)
            return;

        // Debug om te zien dat Update loopt
        Debug.Log("Update → dichtbij pickup: " + nearbyPickup.name);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E ingedrukt → poging tot oppakken");

            string itemName = nearbyPickup.name;

            // Inventory toevoegen
            if (InventoryManager.Instance != null)
            {
                Debug.Log("InventoryManager gevonden → item toevoegen: " + itemName);
                InventoryManager.Instance.AddItem(itemName);
            }
            else
            {
                Debug.LogError("InventoryManager.Instance is NULL! Staat er wel een InventoryManager in de scene?");
            }

            // Special case: dagger
            if (itemName.ToLower().Contains("dagger"))
            {
                Debug.Log("Dagger gedetecteerd → in-hand dagger activeren");

                if (daggerInHand != null)
                {
                    daggerInHand.SetActive(true);
                    Debug.Log("daggerInHand succesvol geactiveerd");
                }
                else
                {
                    Debug.LogError("daggerInHand is NULL! Sleep het object in de Inspector!");
                }
            }

            // Wereld-object verwijderen
            Debug.Log("Wereld-object verwijderen: " + nearbyPickup.name);
            Destroy(nearbyPickup.gameObject);

            nearbyPickup = null;
        }
    }
}