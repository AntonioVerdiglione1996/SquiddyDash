using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraJump : TimedSkill
{
    public float JumpForceMultiplier = 2f;
    public float Duration = 5f;

    private float timer;

    protected override void UpdateBehaviour()
    {
        Controller.JumpForceMultiplier = JumpForceMultiplier;
        timer += Time.deltaTime;
        if(timer >= Duration)
        {
            this.enabled = false;
        }
    }
    protected override void OnStopSkill()
    {
        base.OnStopSkill();
        Controller.JumpForceMultiplier = 1f;
        timer = 0f;
    }
}
