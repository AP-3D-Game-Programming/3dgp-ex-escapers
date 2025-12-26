using UnityEngine;

public class RopeCut : MonoBehaviour
{
    public GameObject chandelier;     
    public GameObject relicPrefab;    
    public bool requiresDagger = true;

    public AudioClip breakSound;      // geluid dat afgespeeld wordt

    private bool playerInside = false;

    private void Start()
    {
        Debug.Log("RopeCut Start: Script actief op object " + gameObject.name);

        if (breakSound == null)
            Debug.Log("RopeCut Start: breakSound is NIET toegewezen");

        if (chandelier == null)
            Debug.Log("RopeCut Start: chandelier is NIET toegewezen");

        if (relicPrefab == null)
            Debug.Log("RopeCut Start: relicPrefab is NIET toegewezen");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: geraakt object " + other.name + " met tag " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter: speler is in de rope trigger");
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: verlaten object " + other.name + " met tag " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerExit: speler heeft de rope trigger verlaten");
            playerInside = false;
        }
    }

    private void Update()
    {
        if (!playerInside)
            return;

        Debug.Log("Update: speler is in de trigger, wacht op input");

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Update: speler drukte op E");

            if (requiresDagger)
            {
                bool hasDagger = InventoryManager.Instance.HasItem("Dagger");
                Debug.Log("Update: speler heeft dagger in inventory: " + hasDagger);

                if (!hasDagger)
                {
                    Debug.Log("Update: speler heeft geen dagger, rope kan niet gesneden worden");
                    return;
                }
            }

            Debug.Log("Update: voorwaarden voldaan, rope wordt gesneden");
            CutRope();
        }
    }

    private void CutRope()
    {
        Debug.Log("CutRope: functie gestart");

        // Breaking sound via PlayClipAtPoint
        if (breakSound != null)
        {
            Debug.Log("CutRope: breakSound wordt afgespeeld met PlayClipAtPoint op positie " + transform.position);
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }
        else
        {
            Debug.Log("CutRope: GEEN geluid afgespeeld, breakSound is null");
        }

        // Hardcoded spawn positie
        Vector3 spawnPos = new Vector3(-5.9514f, 9.993f, -8.7396f);

        if (relicPrefab != null)
        {
            Debug.Log("CutRope: relic wordt gespawned op vaste positie " + spawnPos);
            Instantiate(relicPrefab, spawnPos, Quaternion.identity);
        }

        if (chandelier != null)
        {
            Debug.Log("CutRope: chandelier wordt destroyed");
            Destroy(chandelier);
        }

        Debug.Log("CutRope: rope object wordt destroyed");
        Destroy(gameObject);
    }
}