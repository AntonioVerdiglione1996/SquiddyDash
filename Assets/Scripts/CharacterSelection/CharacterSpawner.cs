using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public void Awake()
    {
        Instantiate(scm.DownloadCurrentCharacter(), transform);
        Destroy(this);
    }
}
