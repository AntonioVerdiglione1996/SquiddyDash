﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactiveSource : MonoBehaviour
{
    public AudioReactiveClip ClipData;
    public AudioSource Source;
    public SoundEvent SoundEvent;
    public bool DebugActive = false;

    private int currentTimestampIndex;
    private float lastTimeStamp;
    private bool waitingForLoop;
    public bool IsSourceValid()
    {
        return Source && ClipData && ClipData.IsValid();
    }
    private void OnEnable()
    {
        lastTimeStamp = 0f;
        currentTimestampIndex = 0;
        waitingForLoop = false;
        if (!IsSourceValid())
        {
            enabled = false;
            return;
        }
        Source.clip = ClipData.Clip;
        Source.time = 0f;
        Source.Play();
        if (SoundEvent)
        {
            SoundEvent.CurrentStrenght = 0;
        }
    }
    private void Update()
    {
        if (!IsSourceValid())
        {
            enabled = false;
            return;
        }
        if (ClipData.Timestamps == null || ClipData.Timestamps.Length == 0 || !Source.isPlaying)
        {
            return;
        }

        float time = Source.time;
        if (time >= lastTimeStamp)
        {
            ReactiveClipData data = ClipData.Timestamps[currentTimestampIndex];
            if (!waitingForLoop && time > data.Timestamp)
            {
                if (SoundEvent)
                {
                    SoundEvent.CurrentStrenght = data.Strenght;
                    SoundEvent.Raise();
                }
#if UNITY_EDITOR
                if (DebugActive)
                {
                    Debug.LogFormat("Soundevent for the {0} timestamp at {1} game time, {2} clip time!", currentTimestampIndex, Time.time, Source.time);
                }
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
    public void SetSourceToTime(float clipTime)
    {
        Source.time = clipTime;
        currentTimestampIndex = 0;
        if (IsSourceValid())
        {
            if (ClipData.Timestamps != null && ClipData.Timestamps.Length > 0)
            {
                currentTimestampIndex = ClipData.Timestamps.Length - 1;
                for (int i = 0; i < ClipData.Timestamps.Length; i++)
                {
                    if (ClipData.Timestamps[i].Timestamp >= clipTime)
                    {
                        currentTimestampIndex = i;
                        break;
                    }
                }
            }
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
