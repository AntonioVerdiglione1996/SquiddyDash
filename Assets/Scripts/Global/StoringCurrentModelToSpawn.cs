using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "SavedStats/StoringModel")]
public class StoringCurrentModelToSpawn : ScriptableObject
{
    public const string Filename = "CurrentModel.json";
    [SerializeField]
    private int index;
    public List<Character> Characters;
    [SerializeField]
    private List<Accessory> accessories;

    private Queue<Accessory> tempQueue = new Queue<Accessory>();

    public int GetIndex()
    {
        return index;
    }
    public List<Accessory> GetAccessories()
    {
        return accessories;
    }
    private void OnEnable()
    {
        //if the file does not exist we serialize this object for the first time.
        if (!Restore())
        {
            SaveToFile();
        }
    }
    public void SetIndexAndAccessories(int index, List<Accessory> accessories)
    {
        this.index = Mathf.Clamp(index, 0, (Characters == null || Characters.Count <= 0) ? 0 : (Characters.Count - 1));
        tempQueue.Clear();
        if (accessories != null)
        {
            for (int i = 0; i < accessories.Count; i++)
            {
                Accessory acc = accessories[i];
                if (acc)
                {
                    tempQueue.Enqueue(acc);
                }
            }
        }
        this.accessories.Clear();
        while (tempQueue.Count > 0)
        {
            this.accessories.Add(tempQueue.Dequeue());
        }
        //every time i click one of the buttons in char selection my program overwrite the file CurrentModel.json
        //with the new value
        SaveToFile();
    }
    public void OnValidate()
    {
        SetIndexAndAccessories(this.index, accessories);
    }
    public void SaveToFile()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, Filename, this, true);
    }
    public Character DownloadCurrentCharacter()
    {
        Character go = null;
        if (Characters == null)
        {
            return go;
        }
        for (int i = 0; i < Characters.Count; i++)
        {
            if (i == index)
            {
                go = Characters[i];
            }
        }
        return go;
    }
    public bool Restore()
    {
        return SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, Filename, this);
    }
}
