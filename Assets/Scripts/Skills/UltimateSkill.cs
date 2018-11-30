using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UltimateSkill : Skill
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

    public Vector3 WallsCustomRepulsion = new Vector3(1f, 1f, 1f);

    public WallsModifier WallsModifier;

    private int lastScoreInvoke = 0;

    protected override void OnDisable()
    {
    }

    protected virtual void SkillLogic()
    {
        GlobalEvent.ParentToTarget(null, Controller.transform);
        Controller.Rb.AddForce(ForceApplied, Mode);

        WallsModifier.SetNewRepulsion(WallsCustomRepulsion,false,false);
    }
    protected override void OnEnable()
    {
        lastScoreInvoke = ScoreSystem.Score;

        ScoreRequirement = (int)(ScoreRequirement * ScoreRequirementMultIncrementPerInvokation);

        SkillLogic();
    }

    protected virtual void Update()
    {
        if(!Controller)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("{0} does not have a valid reference to a squiddy controller" , this);
#endif
        }

        enabled = Controller.IsJumping;

        if (!enabled)
        {
            WallsModifier.ResetRepulsion();
        }
    }

    public override bool IsSkillInvokable()
    {
        return !enabled && (ScoreSystem.Score - lastScoreInvoke >= ScoreRequirement);
    }

    protected override void ResetSkill()
    {
        enabled = false;
        lastScoreInvoke = 0;
        ScoreRequirement = defaultScoreRequirement;
    }
}
