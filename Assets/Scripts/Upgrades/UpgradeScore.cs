using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Upgrades/Score")]
public class UpgradeScore : Upgrade
{
    public ScoreSystem Score;
    public int ScoreIncrease;
    public override void InitPowerup(PowerUp powUp, PowerUpLogic logic)
    {
    }

    public override bool IsPowerupUpgradable(Type type, PowerUpLogic Powerup)
    {
        return true;
    }

    public override bool IsSkillUpgradable(Type type, Skill skill)
    {
        return true;
    }

    public override void PowerUpCollected(Collider player, PowerUp powUp, PowerUpLogic logic)
    {
        Score.UpdateScore(ScoreIncrease);
    }

    public override void ResetPowerup(PowerUp powUp, PowerUpLogic logic)
    {
    }

    public override void SkillStart(Skill skill)
    {
        Score.UpdateScore(ScoreIncrease);
    }

    public override void SkillStop(Skill skill)
    {
    }

    public override void SkillUpdate(Skill skill)
    {
    }
}
