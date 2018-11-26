using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "InGameCurrency")]
public class InGameCurrency : ScriptableObject
{
    public int Currency;

    public void IncreaseAmount(int amount)
    {
        Currency += amount;
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, "Currency.json", this, true);
    }
}
