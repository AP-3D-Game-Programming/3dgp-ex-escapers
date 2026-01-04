using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip soundtrack;
    [SerializeField, Range(0f, 1f)] private float volume = 0.35f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundtrack;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;

        audioSource.Play();
    }

}