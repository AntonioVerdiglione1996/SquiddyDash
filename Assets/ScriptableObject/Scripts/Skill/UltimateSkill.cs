using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Skills/Ultimate/BasicUltimate")]
public class UltimateSkill : Skill
{
    public ScoreSystem ScoreSystem;

    public Vector3 ForceApplied = new Vector3(0.0f, 100.0f, 0.0f);
    public ForceMode Mode = ForceMode.Impulse;

    [NonSerialized]
    public int ScoreRequirement = 10;

    [SerializeField]
    private int defaultScoreRequirement = 10;

    private int lastScoreInvoke = 0;
    protected virtual void SkillLogic(SquiddyController controller)
    {
        controller.Rb.AddForce(ForceApplied, Mode);
    }
    public override bool InvokeSkill(SquiddyController controller, bool bypassIsSkillInvokable)
    {
        if (bypassIsSkillInvokable || IsSkillInvokable(controller))
        {
            if (SpawnedPrefab)
            {
                SpawnedPrefab.SetActive(true);
            }

            lastScoreInvoke = ScoreSystem.Score;

            SkillLogic(controller);

            return true;
        }
        return false;
    }

    public override bool IsSkillInvokable(SquiddyController controller)
    {
        return ScoreSystem.Score - lastScoreInvoke >= ScoreRequirement;
    }

    protected override void ResetSkill(SquiddyController controller)
    {
        lastScoreInvoke = 0;
        ScoreRequirement = defaultScoreRequirement;
    }
}
