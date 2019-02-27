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

    protected override void InternalImproveInvokability(float amount)
    {
        SetCurrentCooldownElapsedTime(GetCurrentCooldownElapsedTime() + amount);
    }

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
    /// Returns the current cooldown timer state
    /// </summary>
    /// <returns>True if enabled</returns>
    public bool IsCurrentCooldownTimerEnabled()
    {
        return coolDownTimer != null && coolDownTimer.Value.Enabled;
    }

    /// <summary>
    /// Sets the current cooldown timer state
    /// </summary>
    /// <param name="isEnabled">True to enable cooldown timer</param>
    public void SetCurrentCooldownTimerEnabled(bool isEnabled)
    {
        if (coolDownTimer == null)
        {
            return;
        }
        TimerData data = coolDownTimer.Value;
        data.Enabled = isEnabled;
        coolDownTimer.Value = data;
    }

    /// <summary>
    /// Returns the current cooldown elapsed time
    /// </summary>
    /// <returns>Elapsed time</returns>
    public float GetCurrentCooldownElapsedTime()
    {
        if (coolDownTimer == null)
        {
            return 0f;
        }
        TimerData data = coolDownTimer.Value;
        return !data.Enabled ? float.MinValue : data.ElapsedTime;
    }

    /// <summary>
    /// Sets the current cooldown elapsed time
    /// </summary>
    /// <param name="newElapsedTime">New elapsed time</param>
    public void SetCurrentCooldownElapsedTime(float newElapsedTime)
    {
        if (coolDownTimer == null)
        {
            return;
        }
        TimerData data = coolDownTimer.Value;
        if (data.Enabled)
        {
            data.ElapsedTime = newElapsedTime;
            coolDownTimer.Value = data;
        }
    }

    /// <summary>
    /// Returns the current cooldown duration
    /// </summary>
    /// <returns>Duration</returns>
    public float GetCurrentCooldownDuration()
    {
        if (coolDownTimer == null)
        {
            return 0f;
        }
        TimerData data = coolDownTimer.Value;
        return !data.Enabled ? float.MinValue : data.Duration;
    }

    /// <summary>
    /// Sets the current cooldown duration
    /// </summary>
    /// <param name="newDuration">new duration</param>
    public void SetCurrentCooldownDuration(float newDuration)
    {
        if (coolDownTimer == null)
        {
            return;
        }
        TimerData data = coolDownTimer.Value;
        if (data.Enabled)
        {
            data.Duration = newDuration;
            coolDownTimer.Value = data;
        }
    }

    /// <summary>
    /// Disables timer
    /// </summary>
    protected override void ResetSkill()
    {
        if (TimeHelper)
        {
            TimeHelper.RemoveTimer(coolDownTimer);
        }
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
