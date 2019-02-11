using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This script is the handler of
public class ShowModelUI : MonoBehaviour
{
    //button prefab that will be instatiated automatically 1 for each character in the list
    public ShowModelButton ButtonPrefab;
    //Reference characterManager > for all callBacks i'll call on button
    public Character_Manager CharacterManager;

    private void Start()
    {
       
        //i get the copy of the list of character in CharacterManager(Var because im Lazy)
        var models = CharacterManager.GetModels();
        //create a button foreach models we have in the list
        foreach (var model in models)
        {
            CreateButtonForModel(model);
        }
    }
    //instantiate the button and initialize it in ShowModelButton.cs
    private void CreateButtonForModel(Transform Model)
    {
        var button = Instantiate(ButtonPrefab);
        //this.transform will be handled by a grid layout
        button.transform.SetParent(this.transform);
        button.transform.localScale = Vector3.one;
        button.transform.localRotation = Quaternion.identity;

        //CAMBIARE SPRITE AI BUTTON CREATI DYNAMIC
        //noi abbiamo il modello quindi:
        //ci cacheiamo character component
        //1 gettare il componente character dal model
        //2 accedere alla variabile icon di character component
        Character charcur = Model.GetComponent<Character>();
        Sprite iconcur = null;
        if(charcur != null && charcur.Describer)
        {
            iconcur = charcur.Describer.Image;
        }
     
        button.Initialize(Model, CharacterManager.EnableModel, iconcur);
    }
    //-----------------------------------------------
}
