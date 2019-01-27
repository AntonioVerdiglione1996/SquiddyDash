using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedWindZone : MonoBehaviour
{
    public WindZoneManipulator Manipulator;
    public WindZone Windzone;

    public Vector2 WindVelocity;
    public Vector2 MaxWindVelocity = new Vector2(5f, 2f);
    public Vector2 MinWindVelocity = new Vector2(-5f, -2f);
    public Vector3 WindVelocity3 { get { return new Vector3(WindVelocity.x, WindVelocity.y, 0f); } }

    private float timer;
    private void OnEnable()
    {
        if (Manipulator)
        {
            Manipulator.OnWindzoneUpdate += OnWindUpdate;
        }
    }
    private void OnDisable()
    {
        if (Manipulator)
        {
            Manipulator.OnWindzoneUpdate -= OnWindUpdate;
        }
    }
    public void OnWindUpdate()
    {
        if (Windzone)
        {
            Windzone.windMain = Manipulator.WindMain;
            Windzone.windPulseFrequency = Manipulator.WindPulseFrequency;
            Windzone.windPulseMagnitude = Manipulator.WindPulseMagnitude;
            Windzone.windTurbulence = Manipulator.WindTurbolence;
        }
        NewWindImpulse();
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer * Manipulator.WindPulseFrequency > 1f)
        {
            NewWindImpulse();
        }
    }
    public void NewWindImpulse()
    {
        timer = 0f;
        //TODO: calculate new wind direction correctly
        Vector2 newWindDirection = UnityEngine.Random.insideUnitCircle.normalized;
        WindVelocity = newWindDirection * (Manipulator.WindMain + Manipulator.WindPulseMagnitude + UnityEngine.Random.Range(-1f, 1f) * Manipulator.WindTurbolence);
        WindVelocity.x = Mathf.Clamp(WindVelocity.x, MinWindVelocity.x, MaxWindVelocity.x);
        WindVelocity.y = Mathf.Clamp(WindVelocity.y, MinWindVelocity.y, MaxWindVelocity.y);
    }
}