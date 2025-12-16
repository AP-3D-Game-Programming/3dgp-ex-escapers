using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Public lijst zichtbaar in de Inspector
    public List<string> inventory = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string itemName)
    {
        inventory.Add(itemName);
        Debug.Log("Item toegevoegd: " + itemName);
    }

    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
}