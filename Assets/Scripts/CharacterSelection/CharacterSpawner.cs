using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public void Awake()
    {
        Character character = Instantiate(scm.DownloadCurrentCharacter(), transform);

        character.CollectAndSpawnSkills(scm.GetAccessories());
        Destroy(this);
    }
}
