using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowModelButton : MonoBehaviour
{
    private Character objectToShow;

    //this will be our actions that we call when click on button
    private Action<Character> ClickAction;


    public void Initialize(Character objectToShow, Action<Character> ClickAction , Sprite Icon)
    {
        this.objectToShow = objectToShow;
        this.ClickAction = ClickAction;
        //THIS ONLY WHEN WE HAVE SPRITES
        if (this.GetComponent<Image>() != null)
        {
            this.GetComponent<Image>().sprite = Icon;
        }
    }
    private void OnEnable()
    {
        //register to the onclick events
        GetComponent<Button>().onClick.AddListener(HandleButtonClick);
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(HandleButtonClick);
    }

    //this is only a wrapper
    private void HandleButtonClick()
    {
        ClickAction(objectToShow);
    }
}
