using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Pickables/Powerup/LimitedSkill")]
public class PowerUpLogicLimited : PowerUpLogic
{
    [Tooltip("Pool used to spawn limited skills for the player")]
    public BasicSOPool LimitedSkillPrefab;
    [Tooltip("If true the MaxUsages set for this power up will override the limited skill's original MaxUsages")]
    public bool OverrideMaxUsages = true;
    [Tooltip("Limited skill's max usages permitted. Minimum is 1")]
    public int MaxUsages = 1;
    public override void PowerUpCollected(Collider player, PowerUp powUp)
    {
        GameObject obj = Spawner.SpawnPrefab(null, LimitedSkillPrefab, player.transform, false);
        LimitedSkill limited = obj.GetComponentInChildren<LimitedSkill>(true);
        if (limited && OverrideMaxUsages)
        {
            limited.SetMaxUsages(MaxUsages);
        }
    }
}
