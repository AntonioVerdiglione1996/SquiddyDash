using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextHandler : MonoBehaviour
{
    //SO where score value is stored
    public ScoreSystem ScoreSystem;

    TextMeshProUGUI text;

    public bool IsGameOverSceneBestScore;

    private int lastScore = 0;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (!text)
        {
            return;
        }

        int score = 0;
        if (!IsGameOverSceneBestScore)
            score = ScoreSystem.Score;
        else
            score = ScoreSystem.BestScore;

        if(score != lastScore)
        {
            lastScore = score;
            text.text = lastScore.ToString();
        }
    }
}
