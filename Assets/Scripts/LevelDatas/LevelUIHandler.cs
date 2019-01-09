using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIHandler : MonoBehaviour
{
    public LevelData LevelData;
    public GlobalEvents GlobalEvents;
    public InGameCurrency Currency;
    public ScoreSystem ScoreSystem;

    public Image Locker;
    public Text ScoreText;

    public void OnClicked()
    {
        if (LevelData.IsUnlocked)
        {
            ScoreSystem.Reset();
            GlobalEvents.SetCurrentLevel(LevelData);
            GlobalEvents.SelectLevelByIndex(LevelData.LevelIndex);
            return;
        }
        if (Currency.ModifyGameCurrencyAmount(-LevelData.UnlockCost))
        {
            LevelData.IsUnlocked = true;
            Locker.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (LevelData.IsUnlocked)
        {
            Locker.gameObject.SetActive(false);
        }
        else
        {
            Locker.gameObject.SetActive(true);
        }
        if(ScoreText)
        {
            ScoreText.text = LevelData.BestScore.ToString();
        }
    }
}
