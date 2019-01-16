using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public Text text;
    public void Start()
    {
        GameObject go = Instantiate(scm.DownloadCurrentCharacter(), transform);
        if (!go)
        {
            Application.Quit();
        }
        text.text = go.name;
        Destroy(this);
    }
}
