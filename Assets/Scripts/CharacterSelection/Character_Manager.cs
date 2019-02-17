using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Manager : MonoBehaviour
{
    public static Character_Manager Instance;

    private List<Transform> models;

    public Text Text;
    public StoringCurrentModelToSpawn SpawnerCharacter;

    public Character GetCurrentModel
    {
        get
        {
            if (!SpawnerCharacter || models == null || SpawnerCharacter.GetCharacterIndex() >= models.Count)
            {
                return null;
            }
            return models[SpawnerCharacter.GetCharacterIndex()].GetComponent<Character>();
        }
    }
    private void OnAccessoryUpdated()
    {
        Character character = GetCurrentModel;
        if (character)
        {
            Accessory[] old_accessories = character.GetComponentsInChildren<Accessory>(true);
            if (old_accessories != null)
            {
                for (int i = 0; i < old_accessories.Length; i++)
                {
                    GameObject.Destroy(old_accessories[i].gameObject);
                }
            }
            character.CollectAndSpawnSkills(SpawnerCharacter.Accessories, SpawnerCharacter.GetAccessoriesIndices(), false);
        }
    }
    private void Awake()
    {
        if (Instance != null)
            Instance = this;

        models = new List<Transform>();

        if (SpawnerCharacter.Characters == null || SpawnerCharacter.Characters.Count <= 0)
        {
            return;
        }

        SpawnerCharacter.OnAccessoryUpdated += OnAccessoryUpdated;

        for (int i = 0; i < SpawnerCharacter.Characters.Count; i++)
        {
            Transform Model = Instantiate(SpawnerCharacter.Characters[i], transform).transform;

            //every model is off as default
            Model.gameObject.SetActive(false);

            models.Add(Model);
            //set always the first active
            if (i == SpawnerCharacter.GetCharacterIndex())
            {
                Model.gameObject.SetActive(true);
                Character character = Model.GetComponent<Character>();
                //first iteration//name and color of the first model
                if (character.Describer)
                {
                    Text.text = character.Describer.Name;
                    Text.color = character.Describer.Color;
                }
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessoriesIndices());
            }
        }
    }

    //Callback activation of Model ->  SetActive
    public void EnableModel(Transform modelToActivate)
    {
        Character character = modelToActivate.GetComponent<Character>();
        for (int i = 0; i < models.Count; i++)
        {
            Transform transformToActivate = models[i];
            bool shouldBeActive = transformToActivate == modelToActivate;
            if (shouldBeActive)
            {
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessoriesIndices());
            }
            transformToActivate.gameObject.SetActive(shouldBeActive);
            //Setting Varius UI Elements
            if (character.Describer)
            {
                Text.text = character.Describer.Name;
                Text.color = character.Describer.Color;
            }
        }
    }
    private void OnDestroy()
    {
        SpawnerCharacter.OnAccessoryUpdated -= OnAccessoryUpdated;
    }
    public List<Transform> GetModels()
    {
        return models;
    }
}
