using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Powerup/LimitedSkill")]
public class PowerUpLogicLimited : PowerUpLogic
{

    public SOPool LimitedSkillPool;
    public bool OverrideMaxUsages = true;
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
