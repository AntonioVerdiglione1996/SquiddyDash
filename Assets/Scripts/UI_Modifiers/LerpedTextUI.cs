using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LerpedTextUI : MonoBehaviour
{
    public Text Text;
    public TextMeshProUGUI TextPro;
    public int StartValue;
    public int EndValue;
    public int CurrentValue;
    public float LerpDuration = 2f;

    private int prevValue;
    private string text;
    private float timer = 0f;

    public void RestartLerp(float alpha = 0f)
    {
        if(!Text && !TextPro)
        {
            return;
        }
        alpha = Mathf.Clamp01(alpha);
        timer = alpha * LerpDuration;
        UpdateText(alpha);
        this.enabled = true;
    }
    private void UpdateText(float alpha)
    {
        CurrentValue = (int)Mathf.Lerp(StartValue, EndValue, alpha);
        if (text == null || text.Length == 0 || CurrentValue != prevValue)
        {
            text = CurrentValue.ToString();
            prevValue = CurrentValue;
            if (Text)
            {
                Text.text = text;
            }
            if (TextPro)
            {
                TextPro.text = text;
            }
        }
    }
    private void Awake()
    {
        this.enabled = false;
    }
    private void OnDisable()
    {
        timer = 0f;
    }
    private void OnValidate()
    {
        if (!TextPro)
        {
            TextPro = GetComponent<TextMeshProUGUI>();
            if (!TextPro)
            {
                Text = GetComponent<Text>();
            }
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        UpdateText(timer / LerpDuration);
        if(timer >= LerpDuration)
        {
            this.enabled = false;
        }
    }
}
