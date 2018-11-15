using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SimpleAudioEvent", menuName = "AudioEvents/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;
    [Range(0f, 1f)]
    public float Volume = 0.5f;
    [Range(0f, 2f)]
    public float Pitch = 1f;
    public bool Loop;

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0)
            return;
        source.clip = clips[Random.Range(0, clips.Length)];

        source.loop = Loop;
        source.playOnAwake = false;
        source.volume = Volume;
        source.pitch = Pitch;

        source.Play();
    }
}