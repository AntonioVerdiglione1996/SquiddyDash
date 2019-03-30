using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIHandler : IIndexable
{
    public const string UnlockButtonMessage = "Unlock price: ";
    public LevelData LevelData;
    public GlobalEvents GlobalEvents;
    public InGameCurrency Currency;
    public ScoreSystem ScoreSystem;

    public Image Locker;
    public Image LevelImage;
    public Text ScoreText;

    public GameObject UnlockUI;
    public Text UnlockButtonText;

    public Text LevelNameText;

    public BasicEvent DeactivateUnlockUIEvent;

    public Button LeaderboardToggleButton;
    public BasicSOPool LeaderboardUIPrefab;

    public Transform LeaderboardEntryParent;

    private List<LeaderboardEntryUI> leadEntries = new List<LeaderboardEntryUI>();

    private void DeactivateUnlockUI()
    {
        UnshowLeaderboard();
        if (UnlockUI)
        {
            UnlockUI.SetActive(false);
            LeaderboardToggleButton.gameObject.SetActive(true);
        }
    }
    public void OnEnable()
    {
        if (DeactivateUnlockUIEvent)
        {
            DeactivateUnlockUIEvent.OnEventRaised += DeactivateUnlockUI;
        }
    }
    public void OnDisable()
    {
        DeactivateUnlockUI();
        if (DeactivateUnlockUIEvent)
        {
            DeactivateUnlockUIEvent.OnEventRaised -= DeactivateUnlockUI;
        }
    }
    public void OnClicked()
    {
        UnshowLeaderboard();
        if (GlobalEvents.StartLevel(LevelData))
        {
            return;
        }
        if (UnlockUI)
        {
            UnlockUI.SetActive(true);
            LeaderboardToggleButton.gameObject.SetActive(false);
        }
    }
    public void ShowLeaderboard()
    {
        for (int i = 0; i < LevelData.Entries.Length; i++)
        {
            LeaderboardEntry entry = LevelData.Entries[i];
            LeaderboardEntryUI ui = Spawner.SpawnPrefab(null, LeaderboardUIPrefab, LeaderboardEntryParent, false).GetComponentInChildren<LeaderboardEntryUI>(true);
            ui.SetEntry(entry);
            leadEntries.Add(ui);
        }
    }
    public void UnshowLeaderboard()
    {
        for (int i = 0; i < leadEntries.Count; i++)
        {
            leadEntries[i].Recycle();
        }
        leadEntries.Clear();
    }
    public void ToggleLeaderboard()
    {
        if(leadEntries.Count == 0)
        {
            ShowLeaderboard();
        }
        else
        {
            UnshowLeaderboard();
        }
    }
    public void UnlockLevel()
    {
        if (Currency.ModifyGameCurrencyAmount(-LevelData.PurchaseInfo.CurrencyCost))
        {
            LevelData.IsPurchased = true;
            Locker.gameObject.SetActive(false);
            DeactivateUnlockUI();
        }
    }

    public void Initialize(LevelData data)
    {
        LevelData = data;
        if(!LevelData)
        {
            this.gameObject.SetActive(false);
            return;
        }
        DeactivateUnlockUI();
        if(LevelNameText)
        {
            LevelNameText.text = LevelData.PurchaseInfo.Describer.Name;
        }
        if (LevelData.PurchaseInfo.IsPurchased)
        {
            Locker.gameObject.SetActive(false);
        }
        else
        {
            Locker.gameObject.SetActive(true);
        }
        if (ScoreText)
        {
            ScoreText.text = LevelData.BestScore.ToString();
        }
        if (LevelImage)
        {
            LevelImage.sprite = LevelData.Describer.Image;
            LevelImage.material = LevelData.Describer.Material;
            LevelImage.color = LevelData.Describer.Color;
        }
        if(UnlockButtonText)
        {
            UnlockButtonText.text = UnlockButtonMessage + LevelData.PurchaseInfo.CurrencyCost.ToString();
        }
    }
}
