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

    private void LateUpdate()
    {
        OnScoreGoal();
    }
    void OnScoreGoal()
    {
        if (milestones == null || milestones.Milestones == null || milestones.Milestones.Length == 0 || currentMilestone >= milestones.Milestones.Length)
        {
            return;
        }

        ScoreMilestone milestone = milestones.Milestones[currentMilestone];
        if (ScoreSystem.Score >= milestone.Score)
        {
            currentMilestone++;
            ScoreSystem.UpdateScore((int)(milestone.ScoreAsRewardMultiplier * milestone.Score));
            Resize(milestone.ScaleMultiplier , milestone.SpeedMultiplier);
            if (OnMilestoneReached)
            {
                OnMilestoneReached.Raise();
            }
        }
    }
    void Resize(Vector3 ScaleMultiplier, float SpeedMultiplier)
    {
        newSpawn.CurrentScaleMultiplier = new Vector3(newSpawn.CurrentScaleMultiplier.x * ScaleMultiplier.x, newSpawn.CurrentScaleMultiplier.y * ScaleMultiplier.y, newSpawn.CurrentScaleMultiplier.y * ScaleMultiplier.y);
        newSpawn.CurrentSpeedMultiplier *= SpeedMultiplier;
    }
}
