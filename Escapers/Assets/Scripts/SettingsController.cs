using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    
    private void Start()
    {
        LoadSettings();
        SetupListeners();
    }
    
    private void SetupListeners()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }
    
    private void LoadSettings()
    {
        // Load audio settings
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }
    
    private void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
    }
    
    private void OnMusicVolumeChanged(float value)
    {
        // TODO: implement music-specific volume control here
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        // TODO: implement SFX-specific volume control here
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
    }
    
    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}