using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Levels/Data")]
public class LevelData : ScriptableObject, IPurchaseObject
{
    public const string Filename = "LevelData";

    public uint BestScore
    {
        get
        {
            return entries == null || entries.Length <= 0 ? 0 : entries[0].Score;
        }
    }

    [SerializeField]
    private Purchaseable purchaseInfo = new Purchaseable();
    public IPurchaseable PurchaseInfo { get { return purchaseInfo; } }
    public IDescriber Describer { get { return PurchaseInfo.Describer; } }
    public bool IsPurchased { get { return PurchaseInfo.IsPurchased; } set { PurchaseInfo.IsPurchased = value; } }

    public int LevelIndex { get { return levelIndex; } }
    public LeaderboardEntry[] Entries { get { return this.entries; } }

    [SerializeField]
    private int levelIndex;
    [SerializeField]
    private string fileNameFull;

    [SerializeField]
    private uint leaderboardTotalEntries = 10;

    [SerializeField]
    private LeaderboardEntry[] entries;
    void OnValidate()
    {
        if (PurchaseInfo == null)
        {
            purchaseInfo = new Purchaseable();
        }
        PurchaseInfo.OnSaveOverride = SaveToFile;
        PurchaseInfo.OnRestoreOverride = Restore;
        Utils.Builder.Clear();
        Utils.Builder.AppendFormat("{0}_{1}_{2}{3}", Filename, LevelIndex, PurchaseInfo.Describer.Name, Utils.JSONExtension);
        fileNameFull = Utils.Builder.ToString(0, Utils.Builder.Length);
        Utils.Builder.Clear();

        if (leaderboardTotalEntries <= 0)
        {
            leaderboardTotalEntries = 1;
        }
        if (entries.Length != leaderboardTotalEntries)
        {
            SetEntries(entries);
        }
        SaveToFile();
    }
    private void Awake()
    {
        OnValidate();
    }
    //restore serialized values
    private void OnEnable()
    {
        PurchaseInfo.OnSaveOverride = SaveToFile;
        PurchaseInfo.OnRestoreOverride = Restore;
        Restore(true);
        if (entries == null)
        {
            entries = new LeaderboardEntry[leaderboardTotalEntries];
        }
        if (entries.Length != leaderboardTotalEntries)
        {
            SetEntries(this.entries);
            SaveToFile();
        }
    }
    public bool IsLeaderboardScore(uint score)
    {
        return entries == null || leaderboardTotalEntries <= 0 ? false : (entries.Length <= 0 ? true : score > entries[entries.Length - 1].Score);
    }
    public int TryAddEntry(uint Score, string Name)
    {
        return TryAddEntry(new LeaderboardEntry(Score, Name));
    }
    public int TryAddEntry(LeaderboardEntry entry)
    {
        if (entry == null || !IsLeaderboardScore(entry.Score))
        {
            return -1;
        }

        int i;
        for (i = 0; i < entries.Length; i++)
        {
            LeaderboardEntry current = entries[i];
            if (entry.Score > current.Score)
            {
                entries[i] = entry;
                for (int j = i + 1; j < entries.Length; j++)
                {
                    LeaderboardEntry temp = entries[j];
                    entries[j] = current;
                    current = temp;
                }

                break;
            }
        }

        SaveToFile();

        return i;
    }
    public void SetEntries(LeaderboardEntry[] entries)
    {
        LeaderboardEntry[] temp = new LeaderboardEntry[leaderboardTotalEntries];
        int size = Mathf.Min(this.entries.Length, entries.Length, (int)leaderboardTotalEntries);
        for (int i = 0; i < size; i++)
        {
            temp[i] = entries[i];
        }
        for (int i = size; i < temp.Length; i++)
        {
            temp[i] = new LeaderboardEntry();
        }
        this.entries = temp;
        ReorderDescendingEntries();
    }
    public bool Restore(bool SaveIfNoFileFound)
    {
        bool found = SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, fileNameFull, this);
        if (!found && SaveIfNoFileFound)
        {
            SaveToFile();
        }
        return found;
    }
    public void SaveToFile()
    {
        if (entries.Length != leaderboardTotalEntries)
        {
            SetEntries(entries);
        }
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, fileNameFull, this, true);
    }
    public void ReorderDescendingEntries()
    {
        if (entries == null)
        {
            return;
        }
        for (int i = 0; i < entries.Length - 1; i++)
        {
            LeaderboardEntry entry = entries[i];
            LeaderboardEntry next = entries[i + 1];
            if (next.Score > entry.Score)
            {
                entries[i] = next;
                entries[i + 1] = entry;
                i = Mathf.Max(-1, i - 2);
            }
        }
    }
}
