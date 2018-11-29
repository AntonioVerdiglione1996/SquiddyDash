using UnityEngine;
public abstract class Skill : MonoBehaviour
{
    public SkillDescriber Describer;

    protected SquiddyController Controller { get; private set; }

    public bool InvokeSkill(bool bypassIsSkillInvokable)
    {
        if(bypassIsSkillInvokable || IsSkillInvokable())
        {
            enabled = true;
            ExecuteSkill();
            return true;
        }
        return false;
    }
    public abstract bool IsSkillInvokable();
    public void Initialize(SquiddyController controller)
    {
        enabled = false;
        this.Controller = controller;
        ResetSkill();
    }

    protected virtual void Awake()
    {
        enabled = false;
    }

    protected abstract void ResetSkill();
    protected abstract void ExecuteSkill();
}
