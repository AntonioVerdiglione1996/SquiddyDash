using System.Collections.Generic;
using System;
using UnityEngine;
public class Invincibility : TimedSkill
{
    public float Duration = 1f;

    public WallsModifier WallsModifier;

    public Vector3 NewRepulsionMultiplier = new Vector3(0.8f, 0.8f, 0.8f);

    protected LinkedListNode<TimerData> durationTimer;

    private Action OnInvincibilityOver;

    private bool successfullWallModification = false;

    protected override void OnDisable()
    {
        base.OnDisable();
        if (WallsModifier && WallsModifier.Walls)
        {
            WallsModifier.Walls.BotWall.isTrigger = true;
            if (successfullWallModification)
            {
                WallsModifier.ResetRepulsion();
            }
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        TimeHelper.RemoveTimer(durationTimer);
        durationTimer = TimeHelper.AddTimer(OnInvincibilityOver, Duration);

        WallsModifier.Walls.BotWall.isTrigger = false;
        successfullWallModification = WallsModifier.SetNewRepulsion(NewRepulsionMultiplier, true, true);
    }

    protected override void ResetSkill()
    {
        base.ResetSkill();
        successfullWallModification = false;
        TimeHelper.RemoveTimer(durationTimer);
        durationTimer = null;
    }

    private void InvincibilityOver()
    {
        enabled = false;
    }

    void Awake()
    {
        OnInvincibilityOver = InvincibilityOver;
    }
}