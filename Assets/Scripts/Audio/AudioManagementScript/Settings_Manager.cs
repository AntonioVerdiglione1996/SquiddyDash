using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/SettingsManager")]
public class Settings_Manager : ScriptableObject
{
    public const string MusicVolume = "Music_Volume";
    public const string EffectsVolume = "Effects_Volume";
    public const string SFXVolume = "Sfx_Volume";
    public AudioMixer AudioMixer;
    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat(MusicVolume, volume);
    }
    public void SetEffectsVolume(float volume)
    {
        AudioMixer.SetFloat(EffectsVolume, volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat(SFXVolume, volume);
    }
}
