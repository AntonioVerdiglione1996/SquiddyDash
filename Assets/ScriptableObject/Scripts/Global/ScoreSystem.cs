using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreSystem", menuName = "ScoreSystem")]
public class ScoreSystem : GameEvent
{
    public const string Filename = "Score.json";
    public const double DefaultScoreMultiplier = 1.0d;
    public int Score { get { return score; } }
    public int BestScore { get { return bestScore; } }
    [NonSerialized]
    public double ScoreMultiplier = DefaultScoreMultiplier;
    [NonSerialized]
    private int score;
    [SerializeField]
    private int bestScore;

    public void ResetScoreMultiplier()
    {
        ScoreMultiplier = DefaultScoreMultiplier;
    }
    private void OnEnable()
    {
        ResetScoreMultiplier();
#if !(UNITY_EDITOR)
        Restore();
#endif
    }
    public void SaveBestScore()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, Filename, this, true);
#if UNITY_EDITOR
        Debug.Log("Saved score");
#endif
    }
    private void Restore()
    {
        SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, Filename, this);
        UpdateScore(0);
    }
    public void UpdateScore(int amountToSum)
    {
        amountToSum = (int)(amountToSum * ScoreMultiplier);

        score += amountToSum;

        if (score > bestScore)
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