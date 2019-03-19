using System;
using UnityEngine;
using System.IO;
public interface IPurchaseObject
{
    IPurchaseable PurchaseInfo { get; }
    IDescriber Describer { get; }
    bool IsPurchased { get; set; }
}
public delegate bool RestoreFile(bool SaveIfNoFileFound);
public interface IPurchaseable
{
    Action OnSaveOverride { get; set; }
    RestoreFile OnRestoreOverride { get; set; }
    string Filename { get; set; }
    string FileDirPath { get; set; }
    IDescriber Describer { get; }
    bool IsPurchased { get; set; }
    int CurrencyCost { get; }
    int AccessoryPartsCost { get; }
    int SkinPartsCost { get; }
    void SaveToFile();
    bool RestoreFromFile(bool SaveIfNoFileFound);
}
[Serializable]
public class Purchaseable : IPurchaseable
{
    public const int UnPurchaseableCost = -1;

    public IDescriber Describer { get { return describer; } }

    public bool IsPurchased
    {
        get { return isUnlocked; }
        set
        {
            if (value != isUnlocked)
            {
                isUnlocked = value;
                SaveToFile();
            }
        }
    }

    public int CurrencyCost { get { return currencyCost; } }

    public int AccessoryPartsCost { get { return accessoryPartsCost; } }

    public int SkinPartsCost { get { return skinPartsCost; } }

    public string Filename { get { return filename; } set { filename = value; } }

    public string FileDirPath { get { return fileDirPath; } set { fileDirPath = value; } }

    public Action OnSaveOverride { get; set; }

    public RestoreFile OnRestoreOverride { get; set; }

    [SerializeField]
    private BaseDescriber describer = new BaseDescriber();
    [SerializeField]
    private bool isUnlocked;
    [SerializeField]
    private int currencyCost;
    [SerializeField]
    private int accessoryPartsCost;
    [SerializeField]
    private int skinPartsCost;
    [SerializeField]
    private string filename;
    [SerializeField]
    private string fileDirPath;

    public Purchaseable() : this(false)
    {

    }
    public Purchaseable(bool isUnlocked, int currencyCost = UnPurchaseableCost, int accessoryPartsCost = UnPurchaseableCost, int skinPartsCost = UnPurchaseableCost, string filename = null, string fileDirPath = null, BaseDescriber describer = null)
    {
        this.isUnlocked = isUnlocked;

        this.currencyCost = currencyCost;
        this.accessoryPartsCost = accessoryPartsCost;
        this.skinPartsCost = skinPartsCost;

        this.describer = describer;
        if (this.describer == null)
        {
            this.describer = new BaseDescriber();
        }

        this.filename = filename;
        if (this.filename == null)
        {
            this.filename = string.Empty;
        }

        this.fileDirPath = fileDirPath;
        if (this.fileDirPath == null)
        {
            this.fileDirPath = string.Empty;
        }
    }
    public bool RestoreFromFile(bool SaveIfNoFileFound)
    {
        if (OnRestoreOverride != null)
        {
            return OnRestoreOverride(SaveIfNoFileFound);
        }

        bool found = SerializerHandler.RestoreObjectFromJson(Path.Combine(SerializerHandler.PersistentDataDirectoryPath, FileDirPath), Filename, this);
        if (!found && SaveIfNoFileFound)
        {
            SaveToFile();
        }
        return found;
    }

    public void SaveToFile()
    {
        if (OnSaveOverride != null)
        {
            OnSaveOverride();
            return;
        }
        SerializerHandler.SaveJsonFromInstance(Path.Combine(SerializerHandler.PersistentDataDirectoryPath, FileDirPath), Filename, this, true);
    }
}