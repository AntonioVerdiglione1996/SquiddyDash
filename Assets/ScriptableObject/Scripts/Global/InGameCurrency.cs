using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "InGameCurrency")]
public class InGameCurrency : ScriptableObject
{
    public const string Filename = "Currency.json";
    public long GameCurrency { get { return gameCurrency; } }
    [SerializeField]
    private long gameCurrency = 0;
    public bool ModifyGameCurrencyAmount(long toSum)
    {
        long result = gameCurrency + toSum;
        if(result < 0)
        {
            return false;
        }
#if UNITY_EDITOR
        Debug.LogFormat("{0} summed to current gamecurrency {1}. Final amount: {2}." , toSum , gameCurrency , result);
#endif
        gameCurrency = result;

        SaveToFile();

        return true;
    }
    public bool Restore()
    {
        return SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, Filename, this);
    }
    public long FetchUpdatedPremiumCurrencyFromServer()
    {
        //TODO: gestire premiumC lato server forse? (per evitare manomissioni lato client).Problema di questo approccio è che una persona offline non possa usare i suoi PremiumCurrency. Dovrebbe essere gestito come operazione asyncrona
        return 0;
    }
    public bool UsePremiumCurrency(long amountUsed)
    {
        if(amountUsed <= 0)
        {
            return false;
        }

        //TODO: inform server of this operation. Dovrebbe essere gestito come operazione asyncrona
        return false;
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
            SaveToFile();
        }
    }
}
