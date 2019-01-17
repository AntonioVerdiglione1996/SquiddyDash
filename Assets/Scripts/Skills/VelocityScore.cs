using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VelocityScore : PassiveSkill
{
    public float PercentageAverageToScore;
    public float AwardIntervall;
    public ScoreSystem Score;

    public float Average { get; private set; }

    private float timer;
    private int previousScore;
    protected override void OnDisable()
    {
    }
    protected override void OnEnable()
    {
        ResetTempVars();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= AwardIntervall)
        {
            Average = Score.Score - previousScore;
            Score.UpdateScore((int)(Average * PercentageAverageToScore));

#if UNITY_EDITOR
            Debug.LogFormat("Skill: Passive skill {0} updated score by {1}!", this, (int)(Average * PercentageAverageToScore));
#endif

            ResetTempVars();
        }
    }
    private void ResetTempVars()
    {
        timer = 0f;
        previousScore = Score.Score;
    }
}