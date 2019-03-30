using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowUpDisplayer : MonoBehaviour
{
    public DescriberEvent OnCollectedEvent;
    public Text Text;
    public TextMeshPro TextPro;
    public Image Image;
    public Text Description;
    public TextMeshPro DescriptionPro;
    public string Prefix = string.Empty;
    public string Suffix = " collected!";
    public float ShowDuration = 2f;

    private float timer;
    private void OnValidate()
    {
        if (!Text && !TextPro)
        {
            TextPro = GetComponent<TextMeshPro>();
            if (!TextPro)
            {
                Text = GetComponent<Text>();
            }
        }
    }
    private void Awake()
    {
        SetActivation(false);
        if (OnCollectedEvent)
        {
            OnCollectedEvent.OnDescriberEvent += OnCollected;
        }
    }
    private void OnDestroy()
    {
        if (OnCollectedEvent)
        {
            OnCollectedEvent.OnDescriberEvent -= OnCollected;
        }
    }
    private void SetActivation(bool isActive)
    {
        enabled = isActive;
        if (Text)
        {
            Text.enabled = isActive;
        }
        if (TextPro)
        {
            TextPro.enabled = isActive;
        }
        if (Description)
        {
            Description.enabled = isActive;
        }
        if (DescriptionPro)
        {
            DescriptionPro.enabled = isActive;
        }
        if (Image)
        {
            Image.enabled = isActive;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > ShowDuration)
        {
            SetActivation(false);
        }
    }
    public void OnCollected(IDescriber describer)
    {
        if (describer != null && (Text || TextPro))
        {
            SetActivation(true);
            timer = 0f;
            Utils.Builder.Clear();
            Utils.Builder.AppendFormat("{0}{1}{2}", Prefix, describer.Name, Suffix);
            string text = Utils.Builder.ToString();
            Utils.Builder.Clear();
            if (Text)
            {
                Text.text = text;
            }
            if (TextPro)
            {
                TextPro.text = text;
            }
            if (Description)
            {
                Description.text = describer.Description;
            }
            if (DescriptionPro)
            {
                DescriptionPro.text = describer.Description;
            }
            if (Image)
            {
                Image.sprite = describer.Image;
                Image.color = describer.Color;
                Image.material = describer.Material;
            }
        }
    }
}
