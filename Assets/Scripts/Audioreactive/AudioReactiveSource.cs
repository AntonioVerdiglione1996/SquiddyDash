using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveSource : MonoBehaviour
{

    public AudioReactiveClip ClipData;
    public AudioSource Source;
    public GameEvent SoundEvent;

    private int currentTimestampIndex;
    private float lastTimeStamp;
    private bool waitingForLoop;
    private void OnEnable()
    {
        lastTimeStamp = 0f;
        currentTimestampIndex = 0;
        waitingForLoop = false;
        if (Source == null || ClipData == null || !ClipData.IsValid())
        {
            enabled = false;
            return;
        }
        Source.clip = ClipData.Clip;
        Source.time = 0f;
        Source.Play();
    }
    private void Update()
    {
        if (ClipData.Timestamps == null || ClipData.Timestamps.Length == 0)
        {
            return;
        }

        float time = Source.time;
        if (time > lastTimeStamp)
        {
            if (!waitingForLoop && time > ClipData.Timestamps[currentTimestampIndex])
            {
                if (SoundEvent)
                {
                    SoundEvent.Raise();
                }
#if UNITY_EDITOR
                Debug.LogFormat("Soundevent for the {0} timestamp!", currentTimestampIndex);
#endif
                currentTimestampIndex++;
            }
        }
        else
        {
            waitingForLoop = false;
        }
        lastTimeStamp = time;

        if (currentTimestampIndex >= ClipData.Timestamps.Length)
        {
            currentTimestampIndex = 0;
            waitingForLoop = true;
        }
    }
    private void OnDisable()
    {
        if (Source)
        {
            Source.Stop();
        }
    }
    public void RefreshAudioClip()
    {
        if (enabled)
        {
            OnEnable();
        }
        enabled = true;
    }
}
