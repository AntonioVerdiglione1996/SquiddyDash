using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreGoal : MonoBehaviour
{
    public NewSpawnPlatform newSpawn;

    public ScoreSystem ScoreSystem;
    public BasicEvent OnMilestoneReached;
    [SerializeField]
    private MilestoneHolder milestones;

    private int currentMilestone = 0;

    private int previousScore = 0;

    private void LateUpdate()
    {
        OnScoreGoal();
    }
    void OnScoreGoal()
    {
        if (milestones == null)
        {
            return;
        }

        ScoreMilestone milestone;
        int targetScore = 0;

        if (milestones.Milestones == null || milestones.Milestones.Length == 0 || currentMilestone >= milestones.Milestones.Length)
        {
            milestone = milestones.DefaultMilestone;
            targetScore += previousScore;
        }
        else
        {
            milestone = milestones.Milestones[currentMilestone];
        }
        targetScore += milestone.Score;

        if (ScoreSystem.Score >= targetScore)
        {
            currentMilestone++;
            ScoreSystem.UpdateScore((int)(milestone.ScoreAsRewardMultiplier * milestone.Score));
            Resize(milestone.ScaleMultiplier, milestone.SpeedMultiplier);
            if (OnMilestoneReached)
            {
                OnMilestoneReached.Raise();
            }
            previousScore = ScoreSystem.Score;
        }
    }
    void Resize(Vector3 ScaleMultiplier, float SpeedMultiplier)
    {
        newSpawn.CurrentScaleMultiplier = new Vector3(newSpawn.CurrentScaleMultiplier.x * ScaleMultiplier.x, newSpawn.CurrentScaleMultiplier.y * ScaleMultiplier.y, newSpawn.CurrentScaleMultiplier.y * ScaleMultiplier.y);
        newSpawn.CurrentSpeedMultiplier *= SpeedMultiplier;
    }
}
