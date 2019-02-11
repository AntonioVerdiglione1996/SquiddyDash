using UnityEngine;
public abstract class Upgrade : ScriptableObject
{
    public Describer Describer;
    public bool OverrideSkill = false;
    public bool OverridePowerup = false;

    public abstract void PowerUpCollected(Collider player, PowerUp powUp, PowerUpLogic logic);
    public abstract void ResetPowerup(PowerUp powUp, PowerUpLogic logic);
    public abstract void InitPowerup(PowerUp powUp, PowerUpLogic logic);

    public abstract void SkillStart(Skill skill);
    public abstract void SkillStop(Skill skill);
    public abstract void SkillUpdate(Skill skill);
}
