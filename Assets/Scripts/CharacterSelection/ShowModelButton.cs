using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowModelButton : MonoBehaviour
{
    private Transform objectToShow;

    //this will be our actions that we call when click on button
    private Action<Transform> ClickAction;


    public void Initialize(Transform objectToShow, Action<Transform> ClickAction , Sprite Icon)
    {
        this.objectToShow = objectToShow;
        this.ClickAction = ClickAction;
        //THIS ONLY WHEN WE HAVE SPRITES
        if (this.GetComponent<Image>() != null)
        {
            this.GetComponent<Image>().sprite = Icon;
        }
    }
    private void Start()
    {
        //register to the onclick events
        GetComponent<Button>().onClick.AddListener(HandleButtonClick);
    }

    //this is only a wrapper
    private void HandleButtonClick()
    {
        ClickAction(objectToShow);
    }
}
