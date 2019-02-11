using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public StoringCurrentModelToSpawn scm;
    public void Awake()
    {
        Character character = Instantiate(scm.DownloadCurrentCharacter(), transform);

        if (scm.Accessories != null)
        {
            for (int i = 0; i < scm.Accessories.Count; i++)
            {
                if (scm.Accessories[i])
                {
                    Accessory accessory = Instantiate(scm.Accessories[i]);
                    if (accessory)
                    {
                        accessory.SetParent(character.GetAccessoryTransform(accessory.Type));
                    }
                }
            }
        }

        character.CollectAndSpawnSkills();
        Destroy(this);
    }
}
