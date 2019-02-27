using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public SquiddyController Controller;
    public void Awake()
    {
        Character character = Instantiate(scm.DownloadCurrentCharacter(), transform);
        if (Controller)
        {
            Controller.OwnedCharacter = character;
            Controller.OwnedCharacter.CollectAndSpawnSkills(scm.Accessories, scm.GetAccessoriesIndices());
        }
        Destroy(this);
    }
}
