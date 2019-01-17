using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScoreSystem", menuName = "Utility/Events/ScoreSystem")]
public class ScoreSystem : GameEvent
{
    public const double DefaultScoreMultiplier = 1.0d;
    public int Score { get { return score; } }
    [NonSerialized]
    public double ScoreMultiplier = DefaultScoreMultiplier;
    [NonSerialized]
    private int score;

    public void Reset()
    {
        ResetScore();
        ResetScoreMultiplier();
    }
    public void ResetScoreMultiplier()
    {
        ScoreMultiplier = DefaultScoreMultiplier;
    }
    private void OnEnable()
    {
        Reset();
    }

    public void UpdateScore(int amountToSum)
    {
        amountToSum = (int)(amountToSum * ScoreMultiplier);

        score += amountToSum;

        Raise();
    }
    public void ResetScore()
    {
        score = 0;
    }
}