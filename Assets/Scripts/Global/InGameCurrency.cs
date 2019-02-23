using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "SavedStats/Currency/Ordinary")]
public class InGameCurrency : ScriptableObject
{
    public const string Filename = "Currency.json";

    public int GameCurrency { get { return gameCurrency; } }
    public int AccessoryParts { get { return accessoryParts; } }
    public int SkinParts { get { return skinParts; } }

    public bool EnableDebug = true;

    [SerializeField]
    private int gameCurrency = 0;
    [SerializeField]
    private int accessoryParts = 0;
    [SerializeField]
    private int skinParts = 0;
    public bool ModifyGameCurrencyAmount(int currencyToSum, int accessoryPartsToSum = 0, int skinPartsToSum = 0)
    {
        int resultCurrency = gameCurrency + currencyToSum;
        int resultSkins = skinParts + skinPartsToSum;
        int resultAccessory = accessoryParts + accessoryPartsToSum;

        if (resultCurrency < 0 || accessoryParts < 0 || skinParts < 0)
        {
            return false;
        }

#if UNITY_EDITOR
        if (EnableDebug)
        {
            Debug.LogFormat("{0} summed to current gamecurrency {1}. Final amount: {2}.", currencyToSum, gameCurrency, resultCurrency);
            Debug.LogFormat("{0} summed to current accessoryParts {1}. Final amount: {2}.", accessoryPartsToSum, accessoryParts, resultAccessory);
            Debug.LogFormat("{0} summed to current skinParts {1}. Final amount: {2}.", skinPartsToSum, skinParts, resultSkins);
        }
#endif

        accessoryParts = resultAccessory;
        skinParts = resultSkins;
        gameCurrency = resultCurrency;

        SaveToFile();

        return true;
    }
    public bool Restore()
    {
        return SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, Filename, this);
    }
    public void SaveToFile()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, Filename, this, true);
    }
    private void OnEnable()
    {
        if (!Restore())
        {
            gameCurrency = 0;
            accessoryParts = 0;
            skinParts = 0;
            SaveToFile();
        }
    }
    private void OnDisable()
    {
        SaveToFile();
    }
}
