using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Accessory : MonoBehaviour
{
    public const string Dirname = "Accessory";
    public Describer Describer;
    public Transform Root;
    public Skill Skill;
    public List<Upgrade> Upgrades = new List<Upgrade>();
    public EAccessoryType Type;
    public EAccessoryRarity Rarity
    {
        get
        {
            EAccessoryRarity type = EAccessoryRarity.None;
            if (Skill)
            {
                if (Skill is PassiveSkill)
                {
                    type |= EAccessoryRarity.Rare;
                }
                else
                {
                    type |= EAccessoryRarity.Legendary;
                }
            }
            else
            {
                type |= EAccessoryRarity.Common;
            }
            if (Upgrades != null && Upgrades.Count > 0)
            {
                type |= EAccessoryRarity.Special;
            }
            return type;
        }
    }
    public string FileNameFull { get { return fileNameFull; } }
    public int UnlockCost { get { return unlockCost; } }
    public int UnlockParts { get { return unlockParts; } }
    public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = value; SaveToFile(); } }
    [SerializeField]
    private string fileNameFull;
    [SerializeField]
    private int unlockCost = 10;
    [SerializeField]
    private int unlockParts = 2;
    [SerializeField]
    private bool isUnlocked = false;
    public bool Restore()
    {
        return SerializerHandler.RestoreObjectFromJson(Path.Combine(SerializerHandler.PersistentDataDirectoryPath, Dirname), fileNameFull, this);
    }
    public void SaveToFile()
    {
        SerializerHandler.SaveJsonFromInstance(Path.Combine(SerializerHandler.PersistentDataDirectoryPath, Dirname), fileNameFull, this, true);
    }
    public void SetParent(Transform parent, bool worldPositionStays = false)
    {
        Reset();
        Root.SetParent(parent, worldPositionStays);
#if UNITY_EDITOR
        Debug.LogFormat("{0} of type {1} and rarity {2} has been parented to {3}", this, Type, Rarity, parent);
#endif
    }
    public void SetParent(Transform parent, Vector3 localPosition)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localScale = localScale;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
        Root.localScale = localScale;
    }
    private void Awake()
    {
        Reset();
        if (Upgrades != null)
        {
            for (int i = Upgrades.Count - 1; i >= 0; i--)
            {
                if (!Upgrades[i])
                {
                    Upgrades.RemoveAt(i);
                }
            }
        }
    }
    private void Reset()
    {
        Utils.Builder.Clear();
        Utils.Builder.AppendFormat("{0}_{1}_{2}{3}", (this.Describer ? this.Describer.Name : name), Rarity, Type, Utils.JSONExtension);
        fileNameFull = Utils.Builder.ToString(0, Utils.Builder.Length).Replace(", ", "_");
        Utils.Builder.Clear();

        if (!Skill)
        {
            Skill = GetComponent<Skill>();
        }
        if (!Root)
        {
            Root = transform;
        }
    }
    private void OnValidate()
    {
        Reset();
        SaveToFile();
    }
}
