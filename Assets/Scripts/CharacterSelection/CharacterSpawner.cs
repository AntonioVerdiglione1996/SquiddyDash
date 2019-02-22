using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public SquiddyController Controller;
    public void Awake()
    {
        Controller.OwnedCharacter = Instantiate(scm.DownloadCurrentCharacter(), transform);
        Controller.OwnedCharacter.CollectAndSpawnSkills(scm.Accessories, scm.GetAccessoriesIndices());
        Destroy(this);
    }
}
