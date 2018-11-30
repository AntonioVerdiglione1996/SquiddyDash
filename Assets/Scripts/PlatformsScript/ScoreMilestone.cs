using System;
using UnityEngine;
[Serializable]
public class ScoreMilestone
{
    public static readonly Vector3 DefaultScaleMultiplier = new Vector3(0.8f,1f,1f);
    public const float DefaultSpeedMultiplier = 1.15f;
    public int Score;
    public int Reward;
    public Vector3 ScaleMultiplier;
    public float SpeedMultiplier;
    public ScoreMilestone(int score, int reward , float speedMultiplier, Vector3 scaleMultiplier)
    {
        Score = score;
        Reward = reward;
        ScaleMultiplier = scaleMultiplier;
        SpeedMultiplier = speedMultiplier;
    }
    public ScoreMilestone(int score, int reward)
    {
        Score = score;
        Reward = reward;
        ScaleMultiplier = DefaultScaleMultiplier;
        SpeedMultiplier = DefaultSpeedMultiplier;
    }
}