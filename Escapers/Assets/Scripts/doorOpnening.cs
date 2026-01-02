using UnityEngine;

public class FinalDoorOpen : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private bool isOpen = false;

    [Header("UI")]
    public GameObject interactText;      // "[E] to open door"
    public GameObject lockedText;        // "The door is sealed by magic"

    private bool playerInside = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private RuneManager runeManager;

    private void Start()
    {
        runeManager = FindObjectOfType<RuneManager>();

        if (interactText != null)
            interactText.SetActive(false);

        if (lockedText != null)
            lockedText.SetActive(false);

        closedRotation = door.rotation;
        openRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            playerInside = true;

            if (runeManager.AllRunesDestroyed())
            {
                if (interactText != null)
                    interactText.SetActive(true);
            }
            else
            {
                if (lockedText != null)
                    lockedText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null)
                interactText.SetActive(false);

            if (lockedText != null)
                lockedText.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInside || isOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!runeManager.AllRunesDestroyed())
            {
                Debug.Log("Deur blijft dicht — runes zijn nog niet vernietigd");
                return;
            }

            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;

        if (interactText != null)
            interactText.SetActive(false);

        StartCoroutine(OpenDoorSmooth());
    }

    private System.Collections.IEnumerator OpenDoorSmooth()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            door.rotation = Quaternion.Slerp(closedRotation, openRotation, t);
            yield return null;
        }
    }
}