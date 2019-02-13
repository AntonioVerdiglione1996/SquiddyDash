using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base class for all skills that uses a cooldown system. It gets enabled when skill is invoked successfully, then it should be disabled in child classes to put it back into cooldown
/// </summary>
public abstract class TimedSkill : Skill
{
    /// <summary>
    /// Cooldown duration (starts when skill gets disabled)
    /// </summary>
    public float CoolDownDuration = 5f;

    /// <summary>
    /// Timer manager
    /// </summary>
    public TimeHelper TimeHelper;

    /// <summary>
    /// cooldown timer associated with the TimeHelper
    /// </summary>
    protected LinkedListNode<TimerData> coolDownTimer = null;

    /// <summary>
    /// Returns how much is left before a new skill invokation is available. Some skills may not fully support this
    /// </summary>
    /// <returns>0f if cooldown over, 1f if cooldown just started. Lerped value between 0 and 1 if supported by skill</returns>
    public override float GetCooldownPassedPercentage()
    {
        if (coolDownTimer == null)
        {
            return 0f;
        }
        TimerData Data = coolDownTimer.Value;
        if (Data.Duration != 0f)
        {
            return Mathf.Clamp01(Data.ElapsedTime / Data.Duration);
        }
        return 1f;
    }

    /// <summary>
    /// Condition checked when an InvokeSkill with bypass = false is requested.
    /// </summary>
    /// <returns>False if cooldown timer either does not exist or if it is not over, true if skill is off cooldown</returns>
    public override bool IsSkillInvokable()
    {
        if (coolDownTimer == null)
        {
            return false;
        }
        TimerData Data = coolDownTimer.Value;
        return (Data.ElapsedTime >= Data.Duration);
    }

    /// <summary>
    /// Disables timer
    /// </summary>
    protected override void ResetSkill()
    {
        TimeHelper.RemoveTimer(coolDownTimer);
        coolDownTimer = null;
    }

    /// <summary>
    /// Starts cooldown phase
    /// </summary>
    protected override void OnStopSkill()
    {
        coolDownTimer = TimeHelper.RestartTimer(null, null, coolDownTimer, CoolDownDuration);
    }
    /// <summary>
    /// Ends cooldown phase
    /// </summary>
    protected override void OnStartSkill()
    {
        ResetSkill();
    }
}
