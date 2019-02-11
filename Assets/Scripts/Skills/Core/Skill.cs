using System.Collections.Generic;
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

    public bool IsSkillAutoActivating { get { return IsAutoActivating; } }

    /// <summary>
    /// Controller associated with this skill when initialized
    /// </summary>
    public SquiddyController Controller { get; private set; }

    /// <summary>
    /// Boolean that determines if this skill should require to be activated by some sort of input. If true skill will be automatically re-activated
    /// </summary>
    [SerializeField]
    protected bool IsAutoActivating = false;

    /// <summary>
    /// List of upgrades applied to this skill
    /// </summary>
    public List<Upgrade> Upgrades = new List<Upgrade>();

    /// <summary>
    /// Invokes this skill and enables this monobehaviour
    /// </summary>
    /// <param name="bypassIsSkillInvokable">if true the skill will be invoked regardless the value of IsSkillInvokable()</param>
    /// <returns>true if skill was invoked , false otherwise</returns>
    public bool InvokeSkill(bool bypassIsSkillInvokable = false)
    {
        if (bypassIsSkillInvokable || IsSkillInvokable())
        {
            enabled = true;
        }
        return enabled;
    }
    /// <summary>
    /// Returns how much is left before a new skill invokation is available. Some skills may not fully support this
    /// </summary>
    /// <returns>0f if cooldown just started, 1f if cooldown is over. Lerped value between 0 and 1 if supported by skill</returns>
    public abstract float GetCooldownPassedPercentage();
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
#if UNITY_EDITOR
        if (Upgrades == null || Upgrades.Count == 0)
        {
            return;
        }
        List<Upgrade> overriders = new List<Upgrade>();
        for (int i = 0; i < Upgrades.Count; i++)
        {
            Upgrade up = Upgrades[i];
            if (up && up.OverrideSkill)
            {
                overriders.Add(up);
            }
        }
        if (overriders.Count > 1)
        {
            Debug.LogErrorFormat("{0} contains {1} overriding upgrades, this may be an undesired state", this, overriders.Count);
        }
#endif
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
    private void Update()
    {
        bool isBaseBehaviourOverrided = false;

        if (Upgrades != null)
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                Upgrade up = Upgrades[i];
                if (up)
                {
                    isBaseBehaviourOverrided = isBaseBehaviourOverrided || up.OverrideSkill;
                    up.SkillUpdate(this);
                }
            }
        }

        if (!isBaseBehaviourOverrided)
        {
            UpdateBehaviour();
        }
    }
    /// <summary>
    /// Updates the skill state
    /// </summary>
    protected abstract void UpdateBehaviour();

    private void OnDisable()
    {
        bool isBaseBehaviourOverrided = false;

        if (Upgrades != null)
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                Upgrade up = Upgrades[i];
                if (up)
                {
                    isBaseBehaviourOverrided = isBaseBehaviourOverrided || up.OverrideSkill;
                    up.SkillStop(this);
                }
            }
        }

        if (!isBaseBehaviourOverrided)
        {
            OnStopSkill();
        }
    }
    /// <summary>
    /// Skill is over
    /// </summary>
    protected abstract void OnStopSkill();
    /// <summary>
    /// Skill starts. Actual logic of the skill when successfully invoked and enabled
    /// </summary>
    protected abstract void OnStartSkill();

    /// <summary>
    /// Method that should be implemented so that it resets the state of the skill
    /// </summary>
    protected abstract void ResetSkill();

    private void OnEnable()
    {
        bool isBaseBehaviourOverrided = false;

        if (Upgrades != null)
        {
            for (int i = 0; i < Upgrades.Count; i++)
            {
                Upgrade up = Upgrades[i];
                if (up)
                {
                    isBaseBehaviourOverrided = isBaseBehaviourOverrided || up.OverrideSkill;
                    up.SkillStart(this);
                }
            }
        }

        if (!isBaseBehaviourOverrided)
        {
            OnStartSkill();
        }
    }
}
