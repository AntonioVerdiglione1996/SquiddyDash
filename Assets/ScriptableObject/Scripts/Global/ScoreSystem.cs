using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreSystem", menuName = "ScoreSystem")]
public class ScoreSystem : ScriptableObject
{
    [NonSerialized]
    public int Score;
    [NonSerialized]
    public int BestScore;

    //public GameEvent OnScoreUpdated;

    //REACTIVATE ON BUILD
    //private void OnEnable()
    //{
    //    Restore();
    //}
    public void OnGameOverActive()
    {
        CheckBestScore();
        SerializerHandler.SaveJsonFile(SerializerHandler.PersistentDataDirectoryPath, "Score.json", JsonUtility.ToJson(this));
    }
    public void Restore()
    {
        SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, "Score.json", this);
    }
    public void UpdateScore()
    {
        Score++;
    }
    public void ReleaseDataForScore()
    {
        Score = 0;
    }
    public void CheckBestScore()
    {
        int previusBestScore = BestScore;
        BestScore = Score > previusBestScore ? Score : previusBestScore;
    }
}
