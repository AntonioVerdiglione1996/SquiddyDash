using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraJump : TimedSkill
{
    public float JumpForceMultiplier = 2f;
    public float Duration = 5f;

    private float timer;

    private void Update()
    {
        Controller.JumpForceMultiplier = JumpForceMultiplier;
        timer += Time.deltaTime;
        if(timer >= Duration)
        {
            this.enabled = false;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.JumpForceMultiplier = 1f;
        timer = 0f;
    }
}
