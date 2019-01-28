using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParticleSystemActivator : MonoBehaviour
{
    public GameObject UI_PS;
    public Image SkillImage;

    private void Awake()
    {
        //always initialize the UI particle system of the skill at false
        if (UI_PS != null)
        {
            UI_PS.SetActive(false);
        }
    }
    private void Update()
    {
        if (SkillImage != null)
        {
            if (SkillImage.fillAmount >= 1)
            {

                UI_PS.SetActive(true);
                UI_PS.GetComponent<UIParticleSystem>().Play();
            }
            else
            {
                UI_PS.SetActive(false);
            }
        }
    }
}
