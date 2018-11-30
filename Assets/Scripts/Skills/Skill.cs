using UnityEngine;
/// <summary>
/// Base skill class. This monobehaviour is disabled by default, gets enabled when skill gets invoked successfully and should be disabled in child classes when skill is over
/// </summary>
public abstract class Skill : MonoBehaviour
{
    /// <summary>
    /// Object holding informations about this skill (no gameplay effect)
    /// </summary>
    public SkillDescriber Describer;

    /// <summary>
    /// Controller associated with this skill when initialized
    /// </summary>
    protected SquiddyController Controller { get; private set; }

    /// <summary>
    /// Invokes this skill and enables this monobehaviour
    /// </summary>
    /// <param name="bypassIsSkillInvokable">if true the skill will be invoked regardless the value of IsSkillInvokable()</param>
    /// <returns>true if skill was invoked , false otherwise</returns>
    public bool InvokeSkill(bool bypassIsSkillInvokable = false)
    {
        if(bypassIsSkillInvokable || IsSkillInvokable())
        {
            enabled = true;
        }
        return enabled;
    }
    /// <summary>
    /// Condition checked when an InvokeSkill with bypass = false is requested.
    /// </summary>
    /// <returns>False to not execute the skil, true if skill can be executed</returns>
    public abstract bool IsSkillInvokable();

    /// <summary>
    /// Controller associated with this skill gets initialized, monobehaviours is disabled and a resetskill gets invoked
    /// </summary>
    /// <param name="controller">Controller to associate with this skill</param>
    public void Initialize(SquiddyController controller)
    {
        this.Controller = controller;
        Reset();
    }

    protected virtual void OnValidate()
    {
        enabled = false;
    }

    private void Reset()
    {
        bool wasEnabled = enabled;

        OnValidate();
        ResetSkill();

        if (!wasEnabled)
        {
            OnDisable();
        }
    }

    /// <summary>
    /// Skill is over
    /// </summary>
    protected abstract void OnDisable();

    /// <summary>
    /// Method that should be implemented so that it resets the state of the skill
    /// </summary>
    protected abstract void ResetSkill();
    /// <summary>
    /// Skill starts. Actual logic of the skill when successfully invoked and enabled
    /// </summary>
    protected abstract void OnEnable();
}
