using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WindZoneManipulator : MonoBehaviour
{
    public int DefaultIntensity = 1;

    public SoundEvent SoundEvent;

    public event Action OnWindzoneUpdate;

    public AnimationCurve IntensityMultiplier;
    public AnimationCurve PulseMultiplier;
    public AnimationCurve TurbolenceMultiplier;
    public AnimationCurve FrequencyMultiplier;
    public float IntervallToDefaultIntensity = 0.15f;

    public int CurrentIntensity { get; private set; }
    public bool IsCurrentIntensityCustom { get { return CurrentIntensity != DefaultIntensity; } }
    public float WindMain { get { return CurrentIntensity * IntensityMultiplier.Evaluate(CurrentIntensity); } }
    public float WindPulseMagnitude { get { return CurrentIntensity * PulseMultiplier.Evaluate(CurrentIntensity); } }
    public float WindTurbolence { get { return CurrentIntensity * TurbolenceMultiplier.Evaluate(CurrentIntensity); } }
    public float WindPulseFrequency { get { return CurrentIntensity * FrequencyMultiplier.Evaluate(CurrentIntensity); } }

    private float timer = 0f;

    public void Awake()
    {
        Initialization();
        if (SoundEvent)
        {
            SoundEvent.OnSoundEvent += SetIntensity;
        }
    }
    public void OnDestroy()
    {
        if (SoundEvent)
        {
            SoundEvent.OnSoundEvent -= SetIntensity;
        }
    }
    public void Update()
    {
        if (!IsCurrentIntensityCustom)
        {
            this.enabled = false;
            return;
        }
        timer += Time.deltaTime;
        if (timer >= IntervallToDefaultIntensity)
        {
            SetIntensity(DefaultIntensity);
        }
    }
    public void SetIntensity(int Intensity)
    {
        timer = 0f;

        bool changed = Intensity != CurrentIntensity;
        CurrentIntensity = Intensity;

        this.enabled = IsCurrentIntensityCustom;

        if(changed && OnWindzoneUpdate != null)
        {
            OnWindzoneUpdate();
        }
    }
    private void Initialization()
    {
        SetWrapModes();
        SetIntensity(DefaultIntensity);
    }
    private void SetWrapModes()
    {
        IntensityMultiplier.postWrapMode = WrapMode.Clamp;
        IntensityMultiplier.preWrapMode = WrapMode.Clamp;
        PulseMultiplier.postWrapMode = WrapMode.Clamp;
        PulseMultiplier.preWrapMode = WrapMode.Clamp;
        TurbolenceMultiplier.postWrapMode = WrapMode.Clamp;
        TurbolenceMultiplier.preWrapMode = WrapMode.Clamp;
        FrequencyMultiplier.postWrapMode = WrapMode.Clamp;
        FrequencyMultiplier.preWrapMode = WrapMode.Clamp;
    }
}