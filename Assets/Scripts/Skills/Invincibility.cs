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

    private RepulsionModificationStatus wallsRepulsionState;

    protected override void OnValidate()
    {
        base.OnValidate();
        if (WallsModifier && wallsRepulsionState != null && wallsRepulsionState.IsValid)
        {
            WallsModifier.ResetRepulsion();
        }
        wallsRepulsionState = new RepulsionModificationStatus(WallsModifier, true, true, NewRepulsionMultiplier);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (WallsModifier && WallsModifier.Walls)
        {
            WallsModifier.Walls.SetCollisionTrigger(true, EWallType.Down);
            if (wallsRepulsionState.IsValid)
            {
                WallsModifier.ResetRepulsion();
            }
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        durationTimer = TimeHelper.RestartTimer(OnInvincibilityOver, null, durationTimer, Duration);

        WallsModifier.Walls.SetCollisionTrigger(false, EWallType.Down);
        WallsModifier.SetNewRepulsion(wallsRepulsionState);
    }

    protected override void ResetSkill()
    {
        base.ResetSkill();
        if (wallsRepulsionState.IsValid)
        {
            WallsModifier.ResetRepulsion();
        }
        TimeHelper.RemoveTimer(durationTimer);
        durationTimer = null;
    }

    private void InvincibilityOver()
    {
        enabled = false;
    }

    void Awake()
    {
        wallsRepulsionState = new RepulsionModificationStatus(WallsModifier, true, true, NewRepulsionMultiplier);
        OnInvincibilityOver = InvincibilityOver;
    }
}