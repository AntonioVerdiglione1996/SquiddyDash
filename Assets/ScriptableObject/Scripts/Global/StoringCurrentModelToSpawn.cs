using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "StoringModel")]
public class StoringCurrentModelToSpawn : ScriptableObject
{
    public const string Filename = "CurrentModel.json";
    public int index;
    public List<GameObject> Characters;

    private void OnEnable()
    {
        //if the file does not exist we serialize this object for the first time.
        if (!Restore())
        {
            SaveToFile();
        }
    }
    public void SetIndex(int index)
    {
        this.index = index;
        //every time i click one of the buttons in char selection my program overwrite the file CurrentModel.json
        //with the new value
        SaveToFile();
    }
    public void OnValidate()
    {
        SaveToFile();
    }
    public void SaveToFile()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, Filename, this, true);
    }
    public GameObject DownloadCurrentCharacter()
    {
        GameObject go = null;
        if(Characters == null)
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
