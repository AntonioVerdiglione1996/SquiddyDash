using System;
using UnityEngine;
[Serializable]
public class ScoreMilestone
{
    public static readonly Vector3 DefaultScaleMultiplier = new Vector3(0.95f,1f,1f);
    public const float DefaultSpeedMultiplier = 1.05f;
    public int Score;
    public float ScoreAsRewardMultiplier = 0.1f;
    public Vector3 ScaleMultiplier;
    public float SpeedMultiplier;
    public ScoreMilestone(int score, float scoreAsRewardMultiplier, float speedMultiplier, Vector3 scaleMultiplier)
    {
        Score = score;
        ScoreAsRewardMultiplier = scoreAsRewardMultiplier;
        ScaleMultiplier = scaleMultiplier;
        SpeedMultiplier = speedMultiplier;
    }
    public ScoreMilestone(int score, float scoreAsRewardMultiplier)
    {
        Score = score;
        ScoreAsRewardMultiplier = scoreAsRewardMultiplier;
        ScaleMultiplier = DefaultScaleMultiplier;
        SpeedMultiplier = DefaultSpeedMultiplier;
    }
}