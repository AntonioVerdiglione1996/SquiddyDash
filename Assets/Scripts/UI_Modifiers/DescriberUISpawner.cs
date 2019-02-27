using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriberUISpawner : MonoBehaviour
{
    public SOPool DescriberUI;
    public Transform UIParent;
    public StoringCurrentModelToSpawn Store;
    public Character_Manager Manager;

    private List<GameObject> spawnedUI = new List<GameObject>();
    // Use this for initialization
    void OnEnable()
    {
        Transform characterTransform = Manager.GetModels()[Store.GetCharacterIndex()];
        Character character = characterTransform.GetComponent<Character>();
        if (!character)
        {
            character = characterTransform.GetComponentInChildren<Character>(true);
        }
        if (!character)
        {
            return;
        }

        Spawn(character.Describer);

        Accessory[] accessories = character.GetComponentsInChildren<Accessory>(true);
        Skill[] skills = character.GetComponentsInChildren<Skill>(true);

        if (accessories != null)
        {
            for (int i = 0; i < accessories.Length; i++)
            {
                Spawn(accessories[i].Describer);
            }
        }
        if (skills != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                Skill skill = skills[i];
                bool valid = true;
                if (accessories != null)
                {
                    for (int j = 0; j < accessories.Length; j++)
                    {
                        if(accessories[j].Skill == skill)
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    Spawn(skill.Describer);
                }
            }
        }
    }
    private void Spawn(Describer describer)
    {
        int nullObj;
        GameObject obj = DescriberUI.Pool.Get(UIParent, out nullObj, true);
        DescriberUI describerUI = obj.GetComponent<DescriberUI>();
        if (!describerUI)
        {
            describerUI = obj.GetComponentInChildren<DescriberUI>(true);
        }
        if (!describerUI)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("{0} pool does not contain a DescriberUI", DescriberUI);
#endif
            GameObject.Destroy(obj);
            return;
        }
        describerUI.SetDescriber(describer);
        spawnedUI.Add(describerUI.gameObject);
    }
    // Update is called once per frame
    void OnDisable()
    {
        for (int i = 0; i < spawnedUI.Count; i++)
        {
            DescriberUI.Pool.Recycle(spawnedUI[i]);
        }
        spawnedUI.Clear();
    }
}
