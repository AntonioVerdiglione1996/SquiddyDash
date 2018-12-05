using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelData")]
public class LevelData : ScriptableObject
{
    public const string Filename = "LevelData";

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
        set
        {
            if(value != isUnlocked)
            {
                isUnlocked = value;
                SaveToFile();
            }
        }
    }
    public long UnlockCost
    {
        get
        {
            return unlockCost;
        }
        set
        {
            if(unlockCost != value)
            {
                unlockCost = value;
                SaveToFile();
            }
        }
    }
    public int BestScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            if (bestScore != value)
            {
                bestScore = value;
                SaveToFile();
            }
        }
    }
    public int LevelIndex { get { return levelIndex; } }

    [SerializeField]
    private int levelIndex;
    [SerializeField]
    private bool isUnlocked;
    [SerializeField]
    private long unlockCost = 10;
    [SerializeField]
    private int bestScore;

    private string fileNameFull;
    void OnValidate()
    {
        fileNameFull = Filename + levelIndex + name;
    }
    private void Awake()
    {
        OnValidate();
    }
    //restore serialized values
    private void OnEnable()
    {
        if (!Restore())
        {
            SaveToFile();
        }
    }
    public void SetValuesAndSave(bool isUnlocked, long unlockCost, int bestScore)
    {
        bool anyChanges = (this.isUnlocked != isUnlocked) || (this.unlockCost != unlockCost) || (this.bestScore != bestScore);

        if (anyChanges)
        {
            this.isUnlocked = isUnlocked;
            this.bestScore = bestScore;
            this.unlockCost = unlockCost;
            SaveToFile();
        }
    }
    public bool Restore()
    {
        return SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, fileNameFull, this);
    }
    public void SaveToFile()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, fileNameFull, this, true);
    }
}
