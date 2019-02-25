﻿using System.Collections;
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
    public SOPool LeaderboardEntryUI;
    public Transform LeaderboardEntryParent;

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
        if (LevelData.IsUnlocked)
        {
            ScoreSystem.Reset();
            GlobalEvents.SetCurrentLevel(LevelData);
            GlobalEvents.SelectLevelByIndex(LevelData.LevelIndex);
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

    }
    public void UnshowLeaderboard()
    {

    }
    public void ToggleLeaderboard()
    {

    }
    public void UnlockLevel()
    {
        if (Currency.ModifyGameCurrencyAmount(-LevelData.UnlockCost))
        {
            LevelData.IsUnlocked = true;
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
            LevelNameText.text = LevelData.name;
        }
        if (LevelData.IsUnlocked)
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
            LevelImage.sprite = LevelData.Image;
            LevelImage.material = LevelData.Material;
            LevelImage.color = LevelData.Color;
        }
        if(UnlockButtonText)
        {
            UnlockButtonText.text = UnlockButtonMessage + LevelData.UnlockCost.ToString();
        }
    }
}
