using UnityEngine;

public class PuzzleInformationTrigger : MonoBehaviour
{
    [SerializeField] private PuzzleData puzzleData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PuzzleInformationManager manager =
            other.GetComponent<PuzzleInformationManager>();

        if (manager != null)
        {
            manager.ShowPuzzleInformation(puzzleData);
        }
    }
}