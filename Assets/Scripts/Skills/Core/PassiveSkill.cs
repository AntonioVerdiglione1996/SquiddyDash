public abstract class PassiveSkill : Skill
{
    public override float GetCooldownPassedPercentage()
    {
        return 1f;
    }
    public override bool IsSkillInvokable()
    {
        return true;
    }
    protected override void ResetSkill()
    {
        IsAutoActivating = true;
        enabled = true;
    }
    protected override void OnValidate()
    {
        base.OnValidate();
        ResetSkill();
    }
}