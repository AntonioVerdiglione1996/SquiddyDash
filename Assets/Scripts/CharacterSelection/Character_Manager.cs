using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Manager : MonoBehaviour
{
    public static Character_Manager Instance;

    private List<Character> models;

    public Text Text;
    public StoringCurrentModelToSpawn SpawnerCharacter;

    public BasicSOPool SkinPool = new BasicSOPool();
    public RectTransform SkinParent;

    private Queue<GameObject> skinsSpawned = new Queue<GameObject>();

    public Character GetCurrentModel
    {
        get
        {
            if (!SpawnerCharacter || models == null || SpawnerCharacter.GetCharacterIndex() >= models.Count)
            {
                return null;
            }
            return models[SpawnerCharacter.GetCharacterIndex()];
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

        models = new List<Character>();

        if (SpawnerCharacter.Characters == null || SpawnerCharacter.Characters.Count <= 0)
        {
            return;
        }

        SpawnerCharacter.OnAccessoryUpdated += OnAccessoryUpdated;

        for (int i = 0; i < SpawnerCharacter.Characters.Count; i++)
        {
            Character character = Instantiate(SpawnerCharacter.Characters[i], transform);
            Transform Model = character.transform;

            //every model is off as default
            Model.gameObject.SetActive(false);

            models.Add(character);
            //set always the first active
            if (i == SpawnerCharacter.GetCharacterIndex())
            {
                Model.gameObject.SetActive(true);
                //first iteration//name and color of the first model
                if (character.Describer != null)
                {
                    Text.text = character.Describer.Name;
                    Text.color = character.Describer.Color;
                }
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessoriesIndices());
            }
        }

        EnableModel(GetCurrentModel, true);
    }
    private void EnableModel(Character modelToActivate, bool start)
    {
        for (int i = 0; i < models.Count; i++)
        {
            Character transformToActivate = models[i];
            bool shouldBeActive = transformToActivate == modelToActivate;
            if (shouldBeActive)
            {
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessoriesIndices());
            }
            transformToActivate.gameObject.SetActive(shouldBeActive);
            //Setting Varius UI Elements
            if (modelToActivate.Describer != null)
            {
                Text.text = modelToActivate.Describer.Name;
                Text.color = modelToActivate.Describer.Color;
            }
        }
        if (!modelToActivate.SkinOf)
        {
            ManageSkins(SpawnerCharacter.GetCharacterIndex());
        }
        else if (start)
        {
            ManageSkins(SpawnerCharacter.Characters.FindIndex(c => c == modelToActivate.SkinOf));
        }
    }
    private void ManageSkins(int originalIndex)
    {
        Character original = SpawnerCharacter.Characters[originalIndex];
        while (skinsSpawned.Count > 0)
        {
            SkinPool.Recycle(skinsSpawned.Dequeue());
        }
        for (int i = 0; i < models.Count; i++)
        {
            Character character = models[i];
            if (character.SkinOf == original)
            {
                GameObject go = Spawner.SpawnPrefab(null, SkinPool, SkinParent, false);
                skinsSpawned.Enqueue(go);
                ShowModelButton button = go.GetComponent<ShowModelButton>();
                if (button)
                {
                    button.Initialize(character, EnableModel);
                }
            }
        }
    }
    //Callback activation of Model ->  SetActive
    public void EnableModel(Character modelToActivate)
    {
        EnableModel(modelToActivate, false);
    }
    private void OnDestroy()
    {
        SpawnerCharacter.OnAccessoryUpdated -= OnAccessoryUpdated;
    }
    public List<Character> GetModels()
    {
        return models;
    }
}
