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

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform Model = transform.GetChild(i);
            //every model is off as default
            Model.gameObject.SetActive(false);

            models.Add(Model);
            //set always the first active
            if (i == 0)
            {
                Model.gameObject.SetActive(true);
                Character character = Model.GetComponent<Character>();
                //first iteration//name and color of the first model
                Text.text = character.Name;
                Text.color = character.colorName;
                SpawnerCharacter.SetIndex(i);
            }
        }
    }

    //Callback activation of Model ->  SetActive
    public void EnableModel(Transform modelToActivate)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var transformToActivate = transform.GetChild(i);
            bool shouldBeActive = transformToActivate == modelToActivate;
            if (shouldBeActive)
                SpawnerCharacter.SetIndex(i);
            transformToActivate.gameObject.SetActive(shouldBeActive);
            //Setting Varius UI Elements
            Character character = modelToActivate.GetComponent<Character>();
            Text.text = character.Name;
            Text.color = character.colorName;
        }
    }
 
    public List<Transform> GetModels()
    {
        return models;
    }
}
