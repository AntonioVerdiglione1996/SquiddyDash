using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonSkillActivator : MonoBehaviour
{
    public Image Image;
    public Skill ActivableSkill;
    public event System.Action OnSkillReady;
    public event System.Action OnSkillInvoked;

    public Image.FillMethod FillMethod = Image.FillMethod.Radial360;
    public Image.Type ImageType = Image.Type.Filled;
    public bool FillClockwise = true;

    private bool skillReady = false;
    // Use this for initialization
    private bool CheckValidState()
    {
        if (!ActivableSkill || !Image)
        {
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }
    private void OnEnable()
    {
        if (CheckValidState())
        {
            Image.sprite = ActivableSkill.Describer.Image;
            Image.color = ActivableSkill.Describer.Color;
            Image.material = ActivableSkill.Describer.Material;
            this.Image.fillMethod = this.FillMethod;
            this.Image.fillClockwise = FillClockwise;
            this.Image.fillAmount = 0f;
            this.Image.type = ImageType;
        }
    }
    private void OnValidate()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
        }
        if (Image)
        {
            Image.sprite = ActivableSkill ? ActivableSkill.Describer.Image : null;
            Image.color = ActivableSkill ? ActivableSkill.Describer.Color : Color.black;
            Image.material = ActivableSkill ? ActivableSkill.Describer.Material : null;
            this.Image.fillMethod = this.FillMethod;
            this.Image.fillClockwise = FillClockwise;
            this.Image.fillAmount = 0f;
            this.Image.type = ImageType;
        }
    }
    public void InvokeSkill()
    {
        if (!CheckValidState())
        {
            return;
        }
        if (ActivableSkill.InvokeSkill())
        {
#if UNITY_EDITOR
            Debug.LogFormat("{0} invoked", this);
#endif
            skillReady = false;
            if (OnSkillInvoked != null)
            {
                OnSkillInvoked();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckValidState())
        {
            Image.fillAmount = ActivableSkill.GetCooldownPassedPercentage();
            if (Mathf.Approximately(Image.fillAmount, 1f))
            {
                if (OnSkillReady != null && !skillReady)
                {
                    OnSkillReady();
                }
                skillReady = true;
            }
        }
    }
}
