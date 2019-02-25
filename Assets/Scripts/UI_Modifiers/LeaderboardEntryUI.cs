using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class LeaderboardEntryUI : ISOPoolable
{
    public Text ScoreText;
    public Text NameText;
    public Text NameAndScoreText;
    public TextMeshPro ScoreTextPro;
    public TextMeshPro NameTextPro;
    public TextMeshPro NameAndScoreTextPro;

    public string ScorePreface = "Score: ";
    public string NamePreface = "Name: ";

    private uint lastScore = 0;
    private string scoreTxt;
    private string entryName;
    private void Awake()
    {
        scoreTxt = ScorePreface + lastScore.ToString();
        entryName = NamePreface;
    }
    public void SetEntry(LeaderboardEntry Entry)
    {
        if (Entry == null)
        {
            return;
        }
        bool anyChanges = false;
        if (lastScore != Entry.Score && (ScoreText || ScoreTextPro || NameAndScoreText || NameAndScoreTextPro))
        {
            anyChanges = true;
            lastScore = Entry.Score;
            scoreTxt = ScorePreface + lastScore.ToString();
            if (ScoreText)
            {
                ScoreText.text = scoreTxt;
            }
            if (ScoreTextPro)
            {
                ScoreTextPro.text = scoreTxt;
            }
        }
        string finalName = NamePreface + Entry.Name;
        if (entryName != finalName && (NameText || NameTextPro || NameAndScoreText || NameAndScoreTextPro))
        {
            anyChanges = true;
            entryName = finalName;
            if (NameText)
            {
                NameText.text = entryName;
            }
            if (NameTextPro)
            {
                NameTextPro.text = entryName;
            }
        }
        if (anyChanges && (NameAndScoreText || NameAndScoreTextPro))
        {
            string fullText = entryName + ". " + scoreTxt;
            if (NameAndScoreText)
            {
                NameAndScoreText.text = fullText;
            }
            if (NameAndScoreTextPro)
            {
                NameAndScoreTextPro.text = fullText;
            }
        }
    }
}
