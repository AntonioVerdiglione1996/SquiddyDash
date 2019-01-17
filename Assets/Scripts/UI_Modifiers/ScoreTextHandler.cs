using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextHandler : MonoBehaviour
{
    //SO where score value is stored
    public ScoreSystem ScoreSystem;

    public TextMeshProUGUI Text;

    private int lastScore = 0;

    private void OnEnable()
    {
        if (!Text)
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
        UpdateScore();
        (ScoreSystem as GameEvent).OnEventRaised += UpdateScore;
    }
    private void OnDisable()
    {
        (ScoreSystem as GameEvent).OnEventRaised -= UpdateScore;
    }
    public void UpdateScore()
    {
        if (!Text)
        {
            return;
        }

        int score = ScoreSystem.Score;

        if (score != lastScore)
        {
            lastScore = score;
            Text.text = lastScore.ToString();

        }
    }
}
