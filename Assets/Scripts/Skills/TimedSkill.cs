using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class TimedSkill : Skill
{
    public float CoolDownDuration = 5f;

    public TimeHelper TimeHelper;

    protected LinkedListNode<TimerData> coolDown = null;

    protected bool isOnCoolDown { get; private set; }

    [SerializeField]
    protected bool startOnCooldown = true;

    private Action OnCooldownOver;

    protected override void ExecuteSkill()
    {
        SkillLogic();

        TimeHelper.RemoveTimer(coolDown);
        coolDown = TimeHelper.AddTimer(OnCooldownOver, CoolDownDuration);
    }

    public override bool IsSkillInvokable()
    {
        return !isOnCoolDown;
    }

    public void CoolDownOver()
    {
        isOnCoolDown = false;
    }

    protected abstract void SkillLogic();

    protected override void ResetSkill()
    {
        TimeHelper.RemoveTimer(coolDown);
        isOnCoolDown = startOnCooldown;
        if (startOnCooldown)
        {
            coolDown = TimeHelper.AddTimer(OnCooldownOver, CoolDownDuration);
        }
    }

    protected override void Awake()
    {
        OnCooldownOver = CoolDownOver;
        base.Awake();
    }
}
