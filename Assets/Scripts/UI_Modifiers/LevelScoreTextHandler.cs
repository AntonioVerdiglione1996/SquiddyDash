using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelScoreTextHandler : MonoBehaviour {

    public GlobalEvents GlobalStats;

    public TextMeshProUGUI Text;

    private uint lastScore = 0;

    private void OnEnable()
    {
        if (!Text)
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
        Update();
    }
    public void Update()
    {
        if (!Text || !GlobalStats.CurrentLevel)
        {
            return;
        }

        uint score = GlobalStats.CurrentLevel.BestScore;

        if (score != lastScore)
        {
            lastScore = score;
            Text.text = lastScore.ToString();
        }
    }
}
