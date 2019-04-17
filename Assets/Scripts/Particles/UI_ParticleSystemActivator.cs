using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ParticleSystemActivator : MonoBehaviour
{
    public UIParticleSystem UI_PS;
    public ButtonSkillActivator SkillActivator;

    private void Awake()
    {
        DisableParticles(null);
        if (SkillActivator)
        {
            SkillActivator.SkillInvoked += DisableParticles;
            SkillActivator.SkillReady += ActivateParticles;
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogErrorFormat("{0} does not have a valid reference to a skill activator!", this);
        }
#endif
    }
    private void OnDestroy()
    {
        if (SkillActivator)
        {
            SkillActivator.SkillInvoked -= DisableParticles;
            SkillActivator.SkillReady -= ActivateParticles;
        }
    }
    public void ActivateParticles(Skill Skill)
    {
        UI_PS.enabled = true;
        UI_PS.Play();
    }
    public void DisableParticles(Skill Skill)
    {
        UI_PS.enabled = false;
    }
}