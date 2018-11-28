using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedSkill : Skill
{

    public GameEvent OnCoolDownOver;

    public float CoolDownDuration = 5f;

    public TimeHelper TimeHelper;

    protected LinkedListNode<TimerData> coolDown = null;

    protected bool isOnCoolDown { get; private set; }

    [SerializeField]
    private bool startOnCooldown = true;

    public override bool InvokeSkill(SquiddyController controller, bool bypassIsSkillInvokable)
    {
        if (bypassIsSkillInvokable || IsSkillInvokable(controller))
        {
            isOnCoolDown = InvokeSkill(controller);
            if (isOnCoolDown)
            {
                if (SpawnedPrefab)
                {
                    SpawnedPrefab.SetActive(true);
                }
                TimeHelper.RemoveTimer(coolDown);
                coolDown = TimeHelper.AddTimer(OnCoolDownOver, CoolDownDuration);
            }
            return isOnCoolDown;
        }
        return false;
    }

    public override bool IsSkillInvokable(SquiddyController controller)
    {
        return !isOnCoolDown;
    }

    public void CoolDownOver()
    {
        isOnCoolDown = false;
    }

    protected abstract bool InvokeSkill(SquiddyController controller);

    protected override void ResetSkill(SquiddyController controller)
    {
        TimeHelper.RemoveTimer(coolDown);
        isOnCoolDown = startOnCooldown;
        if (startOnCooldown)
        {
            coolDown = TimeHelper.AddTimer(OnCoolDownOver, CoolDownDuration);
        }
    }
}
