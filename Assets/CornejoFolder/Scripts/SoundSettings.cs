using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    [Header("Test Audio")]
    public AudioSource sfxTestAudio;
    public AudioSource uiTestAudio;

    private void Start()
    {
        // Load saved settings
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        uiSlider.value = PlayerPrefs.GetFloat("UIVolume", 1f);

        // Apply saved settings
        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetUIVolume(uiSlider.value);

        // Add listeners
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
    }

    private void SetVolume(string parameter, float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f); // Prevents log(0) issues

        if (volume <= 0.0001f)
        {
            audioMixer.SetFloat(parameter, -80f); // Proper mute behavior
        }
        else
        {
            audioMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
        }

        PlayerPrefs.SetFloat(parameter + "Volume", volume);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume("Master", volume);
    }

    public void SetBGMVolume(float volume)
    {
        SetVolume("BGM", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("SFX", volume);

        // Play test sound only if not already playing
        if (!sfxTestAudio.isPlaying) 
        { 
            sfxTestAudio.PlayOneShot(sfxTestAudio.clip); 
        }
    }

    public void SetUIVolume(float volume)
    {
        SetVolume("UI", volume);

        // Play test sound only if not already playing
        if (!uiTestAudio.isPlaying) 
        { 
            uiTestAudio.PlayOneShot(uiTestAudio.clip); 
        }
    }
}
