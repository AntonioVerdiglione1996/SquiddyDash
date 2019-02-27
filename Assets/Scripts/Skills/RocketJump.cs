﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RocketJump : Skill
{
    public ScoreSystem ScoreSystem;

    public GlobalEvents GlobalEvent;

    public Vector3 ForceApplied = new Vector3(0.0f, 50.0f, 0.0f);
    public ForceMode Mode = ForceMode.Impulse;

    [NonSerialized]
    public int ScoreRequirement = 10;


    [SerializeField]
    private int defaultScoreRequirement = 10;

    public float ScoreRequirementMultIncrementPerInvokation = 1.500001f;

    private int lastScoreInvoke = 0;

    private int bonusScore = 0;

    protected override void OnStopSkill()
    {
        lastScoreInvoke = ScoreSystem.Score;
        ScoreRequirement = (int)(ScoreRequirement * ScoreRequirementMultIncrementPerInvokation);
        bonusScore = 0;
    }

    protected virtual void SkillLogic()
    {
        GlobalEvent.ParentToTarget(null, Controller.transform);
        Controller.Rb.AddForce(ForceApplied, Mode);
    }
    protected override void OnStartSkill()
    {
        SkillLogic();
    }

    protected override void UpdateBehaviour()
    {
        if (!Controller)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("{0} does not have a valid reference to a squiddy controller", this);
#endif
        }

        enabled = Controller.IsJumping;

        lastScoreInvoke = ScoreSystem.Score;
    }
    /// <summary>
    /// Returns how much is left before a new skill invokation is available. Some skills may not fully support this
    /// </summary>
    /// <returns>0f if cooldown over, 1f if cooldown just started. Lerped value between 0 and 1 if supported by skill</returns>
    public override float GetCooldownPassedPercentage()
    {
        float num = ScoreSystem.Score - lastScoreInvoke + bonusScore;
        if (ScoreRequirement != 0f)
        {
            return Mathf.Clamp01(num / ScoreRequirement);
        }
        return 1f;
    }
    public override bool IsSkillInvokable()
    {
        return !enabled && (ScoreSystem.Score + bonusScore - lastScoreInvoke >= ScoreRequirement);
    }

    protected override void ResetSkill()
    {
        bonusScore = 0;
        enabled = false;
        lastScoreInvoke = ScoreSystem ? ScoreSystem.Score : 0;
        ScoreRequirement = defaultScoreRequirement;
    }

    protected override void InternalImproveInvokability(float amount)
    {
        bonusScore += (int)amount;
    }
}
