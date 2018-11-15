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

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!IsGameOverSceneBestScore)
            text.text = ScoreSystem.Score.ToString();
        else
            text.text = ScoreSystem.BestScore.ToString();


    }
}
