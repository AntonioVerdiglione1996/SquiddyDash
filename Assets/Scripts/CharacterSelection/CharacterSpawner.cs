using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public void Start()
    {
        Instantiate(scm.DownloadCurrentCharacter(), transform);
        Destroy(this);
    }
}
