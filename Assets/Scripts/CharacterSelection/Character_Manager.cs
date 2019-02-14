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

    private void Awake()
    {
        if (Instance != null)
            Instance = this;

        models = new List<Transform>();

        if (SpawnerCharacter.Characters == null || SpawnerCharacter.Characters.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < SpawnerCharacter.Characters.Count; i++)
        {
            Transform Model = Instantiate(SpawnerCharacter.Characters[i], transform).transform;

            //every model is off as default
            Model.gameObject.SetActive(false);

            models.Add(Model);
            //set always the first active
            if (i == SpawnerCharacter.GetIndex())
            {
                Model.gameObject.SetActive(true);
                Character character = Model.GetComponent<Character>();
                //first iteration//name and color of the first model
                if (character.Describer)
                {
                    Text.text = character.Describer.Name;
                    Text.color = character.Describer.Color;
                }
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessories());
            }
        }
    }

    //Callback activation of Model ->  SetActive
    public void EnableModel(Transform modelToActivate)
    {
        for (int i = 0; i < models.Count - 1; i++)
        {
            Transform transformToActivate = models[i];
            bool shouldBeActive = transformToActivate == modelToActivate;
            if (shouldBeActive)
            {
                SpawnerCharacter.SetIndexAndAccessories(i, SpawnerCharacter.GetAccessories());
            }
            transformToActivate.gameObject.SetActive(shouldBeActive);
            //Setting Varius UI Elements
            Character character = modelToActivate.GetComponent<Character>();
            if (character.Describer)
            {
                Text.text = character.Describer.Name;
                Text.color = character.Describer.Color;
            }
        }
    }

    public List<Transform> GetModels()
    {
        return models;
    }
}
