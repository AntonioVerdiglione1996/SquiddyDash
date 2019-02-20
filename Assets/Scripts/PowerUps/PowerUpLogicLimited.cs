using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpLogicLimited : PowerUpLogic {

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
            limited.SetMaxUsages(MaxUsages);
            limited.Pool = LimitedSkillPool;
        }
    }
}
