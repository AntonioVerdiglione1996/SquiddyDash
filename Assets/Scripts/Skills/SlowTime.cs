using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : TimedSkill
{
    public float SlowTimePercentage = 0.01f;
    public float Duration = 1f;

    private float timer = 0f;
    protected override void UpdateBehaviour()
    {
        NewMovePlatform.SpeedMultiplier = SlowTimePercentage;
        timer += Time.deltaTime;
        if(timer >= Duration)
        {
            this.enabled = false;
        }
    }
    protected override void OnStopSkill()
    {
        base.OnStopSkill();
        NewMovePlatform.SpeedMultiplier = 1f;
        timer = 0f;
    }
}
