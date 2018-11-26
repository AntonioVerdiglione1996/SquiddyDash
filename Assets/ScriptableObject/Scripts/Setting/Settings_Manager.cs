using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SettingsManager")]
public class Settings_Manager : ScriptableObject
{
    public AudioMixer AudioMixer;
    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("Music_Volume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        AudioMixer.SetFloat("Effects_Volume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat("Sfx_Volume", volume);
    }
}
