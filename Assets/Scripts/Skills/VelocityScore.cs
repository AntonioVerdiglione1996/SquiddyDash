using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VelocityScore : PassiveSkill
{
    public float PercentageAverageToScore = 0.2f;
    public float AwardIntervall = 1.0f;
    public int MinAverageForReward = 5;
    public ScoreSystem Score;

    public int Average { get; private set; }

    private float timer;
    private int previousScore;
    protected override void OnStartSkill()
    {
        ResetTempVars();
    }
    protected override void UpdateBehaviour()
    {
        timer += Time.deltaTime;
        if (timer >= AwardIntervall)
        {
            Average = Score.Score - previousScore;
            if (MinAverageForReward <= Average)
            {
                Score.UpdateScore((int)(Average * PercentageAverageToScore));

#if UNITY_EDITOR
                Debug.LogFormat("Skill: Passive skill {0} updated score by {1}!", this, (int)(Average * PercentageAverageToScore));
#endif
            }

            ResetTempVars();
        }
    }
    private void ResetTempVars()
    {
        timer = 0f;
        previousScore = Score.Score;
    }

    protected override void OnStopSkill()
    {
    }
}