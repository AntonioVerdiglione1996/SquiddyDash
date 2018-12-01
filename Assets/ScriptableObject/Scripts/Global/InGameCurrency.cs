using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "InGameCurrency")]
public class InGameCurrency : ScriptableObject
{
    public long GameCurrency { get { return gameCurrency; } }
    [SerializeField]
    private long gameCurrency;
    public bool ModifyGameCurrencyAmount(long toSum)
    {
        long result = gameCurrency + toSum;
        if(result < 0)
        {
            return false;
        }

        gameCurrency = result;
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, "Currency.json", this, true);
        return true;
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
}
