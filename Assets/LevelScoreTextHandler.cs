using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelScoreTextHandler : MonoBehaviour {

    public GlobalEvents GlobalStats;

    public TextMeshProUGUI Text;

    private int lastScore = 0;

    private void OnEnable()
    {
        if (!Text)
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
    }
    public void Update()
    {
        if (!Text || !GlobalStats.CurrentLevel)
        {
            return;
        }

        int score = GlobalStats.CurrentLevel.BestScore;

        if (score != lastScore)
        {
            lastScore = score;
            Text.text = lastScore.ToString();
        }
    }
}
