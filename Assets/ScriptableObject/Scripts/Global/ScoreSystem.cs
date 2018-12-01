using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreSystem", menuName = "ScoreSystem")]
public class ScoreSystem : GameEvent
{
    public int Score { get { return score; } }
    public int BestScore { get { return bestScore; } }
    [NonSerialized]
    private int score;
    [SerializeField]
    private int bestScore;

    private void OnEnable()
    {
#if !(UNITY_EDITOR)
        Restore();
#endif
    }
    public void SaveBestScore()
    {
        SerializerHandler.SaveJsonFile(SerializerHandler.PersistentDataDirectoryPath, "Score.json", JsonUtility.ToJson(this));
#if UNITY_EDITOR
        Debug.Log("Saved score");
#endif
    }
    private void Restore()
    {
        SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, "Score.json", this);
        UpdateScore(0);
    }
    public void UpdateScore(int amountToSum)
    {
        score += amountToSum;

        if(score > bestScore)
        {
            bestScore = score;
        }

        Raise();
    }
    public void ResetScore()
    {
        UpdateScore(-Score);
    }
}