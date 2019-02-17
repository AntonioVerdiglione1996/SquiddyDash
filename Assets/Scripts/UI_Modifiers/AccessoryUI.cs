using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryUI : DescriberUI
{
    public int AccessoryIndex = 0;
    public StoringCurrentModelToSpawn Scm;
    public Button[] Buttons = new Button[0];
    public bool GetChildButtons = true;
    // Use this for initialization
    public void OnValidate()
    {
        if (GetChildButtons)
        {
            Buttons = GetComponentsInChildren<Button>(true);
        }
    }
    public virtual void SetAccessory(int index)
    {
        AccessoryIndex = index;
        if(Buttons != null)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Button button = Buttons[i];
                button.onClick.RemoveListener(OnSetAccessory);
                button.onClick.AddListener(OnSetAccessory);
            }
        }
    }
    public void OnSetAccessory()
    {
        Scm.AddAccessory(AccessoryIndex);
    }
}
