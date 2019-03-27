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


    public void Initialize(Character objectToShow, Action<Character> ClickAction)
    {
        this.objectToShow = objectToShow;
        this.ClickAction = ClickAction;
        //THIS ONLY WHEN WE HAVE SPRITES
        Image img = this.GetComponent<Image>();
        if (img && objectToShow)
        {
            img.sprite = objectToShow.Describer.Image;
            img.color = objectToShow.Describer.Color;
            img.material = objectToShow.Describer.Material;
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
