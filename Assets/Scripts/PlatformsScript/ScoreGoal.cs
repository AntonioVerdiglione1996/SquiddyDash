using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreGoal : MonoBehaviour
{
    public NewSpawnPlatform newSpawn;
    private Vector3 ScaleX = new Vector3(0.5f, 0, 0);

    public ScoreSystem ScoreSystem;
    private void LateUpdate()
    {
        OnScoreGoal();
    }
    void OnScoreGoal()
    {
        if (ScoreSystem.Score == 10 || ScoreSystem.Score == 25
              || ScoreSystem.Score == 50 || ScoreSystem.Score == 65
              || ScoreSystem.Score == 85 || ScoreSystem.Score == 105
              || ScoreSystem.Score == 200 || ScoreSystem.Score == 280)
        {
            Resize();
            //TODO: here maybe we need some check or some addition to score
            ScoreSystem.Score++;
        }
    }
    void Resize()
    {
        for (int i = 0; i < newSpawn.PlatformPrefabListLength; i++)
        {
            newSpawn.PlatformPrefabsList[i].transform.localScale -= ScaleX;
            //increase move speed of 10%
            newSpawn.PlatformPrefabsList[i].GetComponent<NewMovePlatform>().Speed *= 1.15f;
        }
    }
}
