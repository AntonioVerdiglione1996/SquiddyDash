using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreGoal : MonoBehaviour
{
    public NewSpawnPlatform newSpawn;

    public ScoreSystem ScoreSystem;
    [SerializeField]
    private MilestoneHolder milestones;

    private int currentMilestone = 0;

    private void LateUpdate()
    {
        OnScoreGoal();
    }
    void OnScoreGoal()
    {
        if (milestones == null || milestones.Milestones.Length == 0 || currentMilestone >= milestones.Milestones.Length)
        {
            return;
        }

        ScoreMilestone milestone = milestones.Milestones[currentMilestone];
        if (ScoreSystem.Score >= milestone.Score)
        {
            currentMilestone++;
            ScoreSystem.UpdateScore((int)(milestone.ScoreAsRewardMultiplier * milestone.Score));
            Resize(milestone.ScaleMultiplier , milestone.SpeedMultiplier);
        }
    }
    void Resize(Vector3 ScaleMultiplier, float SpeedMultiplier)
    {
        for (int i = 0; i < newSpawn.PlatformListLength; i++)
        {
            GameObject go = newSpawn.PlatformList[i];
            NewMovePlatform mover = go.GetComponentInChildren<NewMovePlatform>();
            Vector3 localScale = mover.transform.localScale;

            mover.transform.localScale = new Vector3(localScale.x * ScaleMultiplier.x, localScale.y * ScaleMultiplier.y, localScale.y * ScaleMultiplier.y);
            mover.Speed *= SpeedMultiplier;
        }
    }
}
