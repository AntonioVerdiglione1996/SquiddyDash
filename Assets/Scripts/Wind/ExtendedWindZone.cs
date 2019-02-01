using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWindZone : MonoBehaviour
{
    public const float InverseMaxByte = 1f / byte.MaxValue;
    public byte DefaultIntensity = 0;

    public SoundEvent SoundEvent;

    public AnimationCurve IntensityCurve;
    public AnimationCurve PulseMagnitudeCurve;
    public AnimationCurve PulseFrequencyCurve;
    public float IntervallToDefaultIntensity = 1f;

    public byte CurrentIntensity { get; private set; }
    public bool IsCurrentIntensityCustom { get { return CurrentIntensity != DefaultIntensity; } }
    public float WindMain { get { return IntensityCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }
    public float WindPulseMagnitude { get { return PulseMagnitudeCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }
    public float WindPulseFrequency { get { return PulseFrequencyCurve.Evaluate(CurrentIntensity * InverseMaxByte); } }

    private float timerToDefault = 0f;

    public Vector3 WindVelocity;
    public Vector3 MaxWindVelocity = new Vector3(255f, 125f, 0f);
    public Vector3 MinWindVelocity = new Vector3(-255f, -4f, 0f);
    public Vector3 DefaultDirection = Vector3.up;
    public Vector3 WindVelocity2D { get { return new Vector3(WindVelocity.x, WindVelocity.y, 0f); } }

    private float timer;
    private void OnEnable()
    {
        Initialization();
        if (SoundEvent)
        {
            SoundEvent.OnSoundEvent += SetIntensity;
        }
    }
    private void OnDisable()
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
            return;
        }
        timerToDefault += Time.deltaTime;
        if (timerToDefault >= IntervallToDefaultIntensity)
        {
            SetIntensity(DefaultIntensity);
        }
    }
    public void SetIntensity(byte Intensity)
    {
        timerToDefault = 0f;

        bool changed = Intensity != CurrentIntensity;
        CurrentIntensity = Intensity;

        if (changed)
        {
            NewWindImpulse();
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
        PulseFrequencyCurve.postWrapMode = WrapMode.Clamp;
        PulseFrequencyCurve.preWrapMode = WrapMode.Clamp;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer * WindPulseFrequency > 1f)
        {
            NewWindImpulse();
        }
    }
    public void NewWindImpulse()
    {
        timer = 0f;

        Vector3 newWindDirection = UnityEngine.Random.insideUnitSphere;

        float impulseIntensity = newWindDirection.magnitude * WindPulseMagnitude;
        newWindDirection = Mathf.Approximately(newWindDirection.normalized.magnitude, 1f) ? newWindDirection.normalized : DefaultDirection.normalized;

        WindVelocity = newWindDirection * (WindMain + impulseIntensity);
        WindVelocity.x = Mathf.Clamp(WindVelocity.x, MinWindVelocity.x, MaxWindVelocity.x);
        WindVelocity.y = Mathf.Clamp(WindVelocity.y, MinWindVelocity.y, MaxWindVelocity.y);
        WindVelocity.z = Mathf.Clamp(WindVelocity.z, MinWindVelocity.z, MaxWindVelocity.z);
    }
}