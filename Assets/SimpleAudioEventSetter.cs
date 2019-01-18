using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimpleAudioEventSetter : MonoBehaviour
{
    public AudioSource Source;
    public SimpleAudioEvent AudioEvent;
    // Use this for initialization
    void OnEnable()
    {
        if (AudioEvent && Source)
        {
            AudioEvent.Source = Source;
            this.enabled = false;
        }
    }

}
