using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "SavedStats/StoringModel")]
public class StoringCurrentModelToSpawn : ScriptableObject
{
    public const string Filename = "CurrentModel.json";
    [SerializeField]
    private int characterIndex;
    public List<Character> Characters = new List<Character>();
    [SerializeField]
    private List<int> accessoriesIndices;
    public List<Accessory> Accessories = new List<Accessory>();

    private Queue<int> tempQueue = new Queue<int>();

    public int GetCharacterIndex()
    {
        return Mathf.Clamp(characterIndex, 0, (Characters == null || Characters.Count <= 0) ? 0 : (Characters.Count - 1));
    }
    public int GetCharacterIndex(int newIndex)
    {
        return Mathf.Clamp(newIndex, 0, (Characters == null || Characters.Count <= 0) ? 0 : (Characters.Count - 1));
    }
    public List<int> GetAccessoriesIndices()
    {
        return accessoriesIndices;
    }
    private void OnEnable()
    {
        //if the file does not exist we serialize this object for the first time.
        if (!Restore())
        {
            SetIndexAndAccessories(characterIndex, accessoriesIndices);
        }
    }
    public void SetIndexAndAccessories(int index, List<int> accessories)
    {
        this.characterIndex = GetCharacterIndex(index);
        tempQueue.Clear();
        if (accessories != null)
        {
            Character character = Characters[characterIndex];
            for (int i = 0; i < accessories.Count; i++)
            {
                int currIndex = accessories[i];
                Accessory accessory = Accessories[currIndex];
                if (currIndex >= 0 && currIndex < Accessories.Count && character && accessory && character.GetAccessoryTransform(accessory.Type))
                {
                    tempQueue.Enqueue(currIndex);
                }
            }
        }
        this.accessoriesIndices.Clear();
        while (tempQueue.Count > 0)
        {
            int indexToAdd = tempQueue.Dequeue();
            if (!accessoriesIndices.Contains(indexToAdd))
            {
                this.accessoriesIndices.Add(indexToAdd);
            }
        }
        //every time i click one of the buttons in char selection my program overwrite the file CurrentModel.json
        //with the new value
        SaveToFile();
    }
    public void OnValidate()
    {
        SetIndexAndAccessories(this.characterIndex, accessoriesIndices);
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
            if (i == characterIndex)
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
