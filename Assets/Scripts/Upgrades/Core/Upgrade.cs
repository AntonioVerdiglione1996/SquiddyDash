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

    public bool IsSkillUpgradable<T>(T Skill) where T : Skill
    {
        return IsSkillUpgradable(typeof(T), Skill);
    }
    public abstract bool IsSkillUpgradable(System.Type type, Skill skill);
    public bool IsPowerupUpgradable<T>(T Powerup) where T : PowerUpLogic
    {
        return IsPowerupUpgradable(typeof(T), Powerup);
    }
    public abstract bool IsPowerupUpgradable(System.Type type, PowerUpLogic Powerup);
}
