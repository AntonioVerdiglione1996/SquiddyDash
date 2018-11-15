using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public void Awake()
    {
        GameObject go = Instantiate(scm.DownloadCurrentCharacter());
        go.transform.SetParent(transform);
    }
}
