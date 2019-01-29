using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WindZoneManipulator : MonoBehaviour
{
    public const float InverseMaxByte = 1f / byte.MaxValue;
    public byte DefaultIntensity = 0;

    public SoundEvent SoundEvent;

    public event Action OnWindzoneUpdate;

    public AnimationCurve IntensityCurve;
    public AnimationCurve PulseMagnitudeCurve;
    public AnimationCurve TurbolenceCurve;
    public AnimationCurve PulseFrequencyCurve;
    public float IntervallToDefaultIntensity = 0.5f;

    public byte CurrentIntensity { get; private set; }
    public bool IsCurrentIntensityCustom { get { return CurrentIntensity != DefaultIntensity; } }
    public float WindMain { get { return IntensityCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }
    public float WindPulseMagnitude { get { return PulseMagnitudeCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }
    public float WindTurbolence { get { return TurbolenceCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }
    public float WindPulseFrequency { get { return PulseFrequencyCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }

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
    public void SetIntensity(byte Intensity)
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
        IntensityCurve.postWrapMode = WrapMode.Clamp;
        IntensityCurve.preWrapMode = WrapMode.Clamp;
        PulseMagnitudeCurve.postWrapMode = WrapMode.Clamp;
        PulseMagnitudeCurve.preWrapMode = WrapMode.Clamp;
        TurbolenceCurve.postWrapMode = WrapMode.Clamp;
        TurbolenceCurve.preWrapMode = WrapMode.Clamp;
        PulseFrequencyCurve.postWrapMode = WrapMode.Clamp;
        PulseFrequencyCurve.preWrapMode = WrapMode.Clamp;
    }
}