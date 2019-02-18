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

    public event System.Action OnAccessoryUpdated;

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
            VerifyAndSave();
        }
    }
    public void RemoveAccessory(EAccessoryType Type)
    {
        for (int i = 0; i < accessoriesIndices.Count; i++)
        {
            int index = accessoriesIndices[i];
            if (Accessories[index].Type == Type)
            {
                RemoveAccessory(index);
                return;
            }
        }
    }
    public void RemoveAccessory(int accessoryIndex)
    {
        Character character = Characters[characterIndex];
        bool modified = false;
        if (character && accessoryIndex >= 0 && accessoryIndex < Accessories.Count)
        {
            for (int i = accessoriesIndices.Count - 1; i >= 0; i--)
            {
                if (accessoryIndex == accessoriesIndices[i])
                {
                    accessoriesIndices.RemoveAt(i);
                    modified = true;
                }
            }
        }
        if (modified)
        {
            VerifyAndSave();
        }
    }
    public void AddAccessory(int accessoryIndex)
    {
        Character character = Characters[characterIndex];
        if (character && accessoryIndex >= 0 && accessoryIndex < Accessories.Count)
        {
            Accessory toAdd = Accessories[accessoryIndex];
            for (int i = accessoriesIndices.Count - 1; i >= 0; i--)
            {
                Accessory accessory = Accessories[accessoriesIndices[i]];
                if (!accessory || accessory.Type == toAdd.Type)
                {
                    accessoriesIndices.RemoveAt(i);
                }
            }
            accessoriesIndices.Add(accessoryIndex);
            VerifyAndSave();
        }
    }
    public void VerifyAndSave()
    {
        SetIndexAndAccessories(characterIndex, accessoriesIndices);
    }
    public void SetIndexAndAccessories(int index, List<int> accessories)
    {
        for (int i = Accessories.Count - 1; i > 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (Accessories[i] == Accessories[j])
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
        this.characterIndex = GetCharacterIndex(index);
        tempQueue.Clear();
        if (accessories != null)
        {
            Character character = Characters[characterIndex];
            for (int i = 0; i < accessories.Count; i++)
            {
                int currIndex = accessories[i];
                if (currIndex >= 0 && currIndex < Accessories.Count && character)
                {
                    Accessory accessory = Accessories[currIndex];
                    if (accessory && character.GetAccessoryTransform(accessory.Type))
                    {
                        tempQueue.Enqueue(currIndex);
                    }
                }
            }
        }
        this.accessoriesIndices.Clear();
        while (tempQueue.Count > 0)
        {
            int indexToAdd = tempQueue.Dequeue();
            if (!accessoriesIndices.Contains(indexToAdd) /*&& !Accessories.Find(acc => acc != Accessories[indexToAdd] && acc.Type == Accessories[indexToAdd].Type)*/)
            {
                this.accessoriesIndices.Add(indexToAdd);
            }
        }
        //every time i click one of the buttons in char selection my program overwrite the file CurrentModel.json
        //with the new value
        SaveToFile();

        if (OnAccessoryUpdated != null)
        {
            OnAccessoryUpdated();
        }
    }
    public void OnValidate()
    {
        VerifyAndSave();
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
