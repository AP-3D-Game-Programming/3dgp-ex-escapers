using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    // Onzichtbare inventory (lijst van opgepakte objecten)
    private List<string> inventory = new List<string>();

    // Hoe dichtbij je moet zijn om iets op te pakken
    public float pickupRange = 2f;

    void Update()
    {
        // Check of speler op "E" drukt
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        // Zoek alle colliders in de buurt
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider hit in hits)
        {
            // Check of het object de juiste tag heeft
            if (hit.CompareTag("Pickup"))
            {
                // Voeg toe aan inventory (bijvoorbeeld de naam van het object)
                inventory.Add(hit.gameObject.name);

                // Verwijder het object uit de wereld
                Destroy(hit.gameObject);

                Debug.Log("Opgepakt: " + hit.gameObject.name);
                break; // Stop na het eerste object
            }
        }
    }

    // Optioneel: functie om te checken of speler een item heeft
    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
}