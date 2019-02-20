using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Powerup/LimitedSkill")]
public class PowerUpLogicLimited : PowerUpLogic
{
    [Tooltip("Pool used to spawn limited skills for the player")]
    public SOPool LimitedSkillPool;
    [Tooltip("If true the MaxUsages set for this power up will override the limited skill's original MaxUsages")]
    public bool OverrideMaxUsages = true;
    [Tooltip("Limited skill's max usages permitted. Minimum is 1")]
    public int MaxUsages = 1;
    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        int nullObj;
        GameObject obj = LimitedSkillPool.Get(player.transform, out nullObj, true);
        LimitedSkill limited = obj.GetComponentInChildren<LimitedSkill>(true);
        if (limited)
        {
            if (OverrideMaxUsages)
            {
                limited.SetMaxUsages(MaxUsages);
            }
            limited.Pool = LimitedSkillPool;
        }
    }
}
