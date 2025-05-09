using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set music volume
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MyExposedParam", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    // Set SFX volume
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("MyExposedParam 1", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void Start()
    {
        // Load saved volume values
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SetMusicVolume(musicVolume);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SetSFXVolume(sfxVolume);

        // Initialize sliders with saved volume settings
        if (musicSlider != null) musicSlider.value = musicVolume;
        if (sfxSlider != null) sfxSlider.value = sfxVolume;

        // Set slider listeners
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
}
