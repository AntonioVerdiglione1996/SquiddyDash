using System.Collections.Generic;
using System;
using UnityEngine;
public class Invincibility : TimedSkill
{
    public float Duration = 1f;

    protected LinkedListNode<TimerData> durationTimer;

    private Walls walls;

    private Action OnInvincibilityOver;

    private void OnDisable()
    {
        if (walls)
        {
            walls.BotWall.isTrigger = true;
        }

        if (coolDown != null)
        {
            TimerData data = coolDown.Value;
            data.Enabled = true;
            coolDown.Value = data;
        }
    }
    protected override void SkillLogic()
    {
#if UNITY_EDITOR
        Debug.Log("Invincibility");
#endif
        TimeHelper.RemoveTimer(durationTimer);
        durationTimer = TimeHelper.AddTimer(OnInvincibilityOver, Duration);

        walls.BotWall.isTrigger = false;

        if (coolDown != null)
        {
            TimerData data = coolDown.Value;
            data.Enabled = false;
            coolDown.Value = data;
        }
    }

    protected override void ResetSkill()
    {
        base.ResetSkill();
        TimeHelper.RemoveTimer(durationTimer);
    }

    private void InvincibilityOver()
    {
#if UNITY_EDITOR
        Debug.Log("Invincibility over");
#endif
        enabled = false;
    }

    protected override void Awake()
    {
        walls = FindObjectOfType<Walls>();
        OnInvincibilityOver = InvincibilityOver;
        base.Awake();
    }
}