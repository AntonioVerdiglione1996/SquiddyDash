using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreGoal : MonoBehaviour
{
    public NewSpawnPlatform newSpawn;

    public ScoreSystem ScoreSystem;
    [SerializeField]
    private ScoreMilestone[] milestones = new ScoreMilestone[]
    {
        new ScoreMilestone(10,1),new ScoreMilestone(25,5),new ScoreMilestone(50,10),new ScoreMilestone(85,20),
        new ScoreMilestone(130,30),new ScoreMilestone(190,80),new ScoreMilestone(330,100),new ScoreMilestone(500,500),
        new ScoreMilestone(1200,500),new ScoreMilestone(2000,500),new ScoreMilestone(2800,500),new ScoreMilestone(3500,500),
        new ScoreMilestone(4200,500),new ScoreMilestone(4900,500),new ScoreMilestone(5600,500),new ScoreMilestone(6400,500),
        new ScoreMilestone(7100,500),new ScoreMilestone(7700,500),new ScoreMilestone(8800,500),new ScoreMilestone(9400,500),
        new ScoreMilestone(10000,1000),new ScoreMilestone(11500,1000),new ScoreMilestone(13000,1000),new ScoreMilestone(14500,1000),
        new ScoreMilestone(15600,1000),new ScoreMilestone(16700,1000),new ScoreMilestone(17800,1000),new ScoreMilestone(18900,1000),
        new ScoreMilestone(20000,2000),new ScoreMilestone(23000,2000),new ScoreMilestone(26000,2000),new ScoreMilestone(29000,2000),
        new ScoreMilestone(32000,2000),new ScoreMilestone(35000,2000),new ScoreMilestone(38000,2000),new ScoreMilestone(41000,2000),
        new ScoreMilestone(44000,2000),new ScoreMilestone(47000,2000),new ScoreMilestone(50000,2000),new ScoreMilestone(100000,100000)
    };

    private int currentMilestone = 0;

    private void LateUpdate()
    {
        OnScoreGoal();
    }
    void OnScoreGoal()
    {
        if (milestones == null || milestones.Length == 0 || currentMilestone >= milestones.Length)
        {
            return;
        }

        ScoreMilestone milestone = milestones[currentMilestone];
        if (ScoreSystem.Score >= milestone.Score)
        {
            currentMilestone++;
            ScoreSystem.UpdateScore(milestone.Reward);
            Resize(milestone.ScaleMultiplier , milestone.SpeedMultiplier);
        }
    }
    void Resize(Vector3 ScaleMultiplier, float SpeedMultiplier)
    {
        for (int i = 0; i < newSpawn.PlatformPrefabListLength; i++)
        {
            GameObject go = newSpawn.PlatformPrefabsList[i];
            NewMovePlatform mover = go.GetComponentInChildren<NewMovePlatform>();
            Vector3 localScale = mover.transform.localScale;

            mover.transform.localScale = new Vector3(localScale.x * ScaleMultiplier.x, localScale.y * ScaleMultiplier.y, localScale.y * ScaleMultiplier.y);
            mover.Speed *= SpeedMultiplier;
        }
    }
}
