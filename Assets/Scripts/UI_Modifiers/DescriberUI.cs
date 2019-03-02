using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriberUI : MonoBehaviour
{
    public const string DefaultName = "Missing Name";
    public const string DefaultDescription = "Missing Description";
    public static readonly Color DefaultColor = Color.white;
    public IDescriber Describer;
    public Text Name;
    public Text Description;
    public Image Image;
    void Start()
    {
        if (Describer != null)
        {
            SetDescriber(Describer);
        }
    }
    public virtual void SetDescriber(IDescriber describer)
    {
        Describer = describer;
        if (Image)
        {
            Image.sprite = Describer != null ? Describer.Image : null;
            Image.color = Describer != null ? Describer.Color : DefaultColor;
        }
        if (Name)
        {
            Name.text = Describer != null ? Describer.Name : DefaultName;
        }
        if (Description)
        {
            Description.text = Describer != null ? Describer.Description : DefaultDescription;
        }
    }
}
