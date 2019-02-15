using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriberUI : MonoBehaviour
{
    public const string DefaultName = "Missing Name";
    public const string DefaultDescription = "Missing Description";
    public static readonly Color DefaultColor = Color.white;
    public Describer Describer;
    public Text Name;
    public Text Description;
    public Image Image;
    void Start()
    {
        if (Describer)
        {
            SetDescriber(Describer);
        }
    }
    public void SetDescriber(Describer describer)
    {
        Describer = describer;
        if (Image)
        {
            Image.sprite = Describer ? Describer.Image : null;
            Image.color = Describer ? Describer.Color : DefaultColor;
        }
        if (Name)
        {
            Name.text = Describer ? Describer.Name : DefaultName;
        }
        if (Description)
        {
            Description.text = Describer ? Describer.Description : DefaultDescription;
        }
    }
}
