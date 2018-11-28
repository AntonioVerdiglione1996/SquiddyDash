using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "StoringModel")]
public class StoringCurrentModelToSpawn : ScriptableObject
{
    public int index;
    public List<GameObject> Characters;

    private void OnEnable()
    {
        //if the file does not exist we serialize this object for the first time.
        if (!File.Exists(Path.Combine(SerializerHandler.PersistentDataDirectoryPath, "CurrentModel.json")))
        {
            SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, "CurrentModel.json", this, true);
        }
        else //the file already exist so we restore the value
        {
            Restore();
        }
    }
    public void SetIndex(int index)
    {
        this.index = index;
        //every time i click one of the buttons in char selection my program overwrite the file CurrentModel.json
        //with the new value
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, "CurrentModel.json", this, true);
    }
    public void OnValidate()
    {
        SerializerHandler.SaveJsonFromInstance(SerializerHandler.PersistentDataDirectoryPath, "CurrentModel.json", this, true);
    }
    public GameObject DownloadCurrentCharacter()
    {
        GameObject go = null;
        for (int i = 0; i < Characters.Count; i++)
        {
            if (i == index)
            {
                go = Characters[i];
            }
        }
        return go;
    }
    public void Restore()
    {
        SerializerHandler.RestoreObjectFromJson(SerializerHandler.PersistentDataDirectoryPath, "CurrentModel.json", this);
    }
}
