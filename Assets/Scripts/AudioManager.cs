using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager instance;
    [Header("AudioMixer")]
    public AudioMixer mixer;
    [Header("Sliders")]
    public Slider musicslider;
    public Slider SFXslider;

    private const string MusicVolumePARM = "MusicVolume";
    private const string SFXVolumePARM = "SFXVolume";

    private const string MUSIC_VOLUME_PREF = "MusicVolume";
    private const string SFX_VOLUME_PREF = "SFXVolume";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        LoadVolumes();



        if (musicslider != null)
        {
            musicslider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (SFXslider != null)
        {
            SFXslider.onValueChanged.AddListener(SetSFXVolume);
        }
    }
    
    void LoadVolumes()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF, 0.75f);
        float SFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_PREF, 0.75f);

        if (musicslider != null)
        {
            musicslider.value = musicVolume;
        }
        else
        {
            SetMusicVolume(musicVolume);
        }
        if (SFXslider  != null)
        {
            SFXslider.value = SFXVolume;
        }
        else
        {
            SetSFXVolume(SFXVolume);
        }
    }
    // Update is called once per frame
    void SetMusicVolume(float sliderValue)
    {
        float VolumeDB = ConvertToDecibel(sliderValue);
        mixer.SetFloat(MusicVolumePARM, VolumeDB);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PREF, sliderValue);
    }

    void SetSFXVolume(float sliderValue)
    {
        float VolumeDB = ConvertToDecibel(sliderValue);
        mixer.SetFloat(SFXVolumePARM, VolumeDB);
        PlayerPrefs.SetFloat(SFX_VOLUME_PREF, sliderValue);
    }

    private const float DefaultVolume = 0.75f;

    /// <summary>
    /// Resets music and SFX volumes to their default values, updates the sliders, and saves to PlayerPrefs.
    /// </summary>
    public void ResetSoundSettings()
    {
        PlayerPrefs.DeleteKey(MUSIC_VOLUME_PREF);
        PlayerPrefs.DeleteKey(SFX_VOLUME_PREF);

        if (musicslider != null)
            musicslider.value = DefaultVolume;
        else
            SetMusicVolume(DefaultVolume);

        if (SFXslider != null)
            SFXslider.value = DefaultVolume;
        else
            SetSFXVolume(DefaultVolume);

        PlayerPrefs.Save();
    }

    private float ConvertToDecibel(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
        {
            return -80f;
        }

        return Mathf.Log10(sliderValue) * 20;
    }
}
