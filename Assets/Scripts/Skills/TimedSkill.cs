using System.Collections.Generic;
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
    protected override void OnDisable()
    {
        coolDownTimer = TimeHelper.AddTimer(null, null, CoolDownDuration);
    }
    /// <summary>
    /// Ends cooldown phase
    /// </summary>
    protected override void OnEnable()
    {
        ResetSkill();
    }
}
