using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedSkill : ISOPoolable
{
    public Skill Skill;
    public int MaxUsages = 1;
    public bool ActivateImmediatly = true;
    public bool DebugEnabled = true;
    public SquiddyController Controller
    {
        get
        {
            if (!controller)
            {
                controller = GetComponent<SquiddyController>();
                if (!controller)
                {
                    controller = transform.root.GetComponentInChildren<SquiddyController>(true);
                    if (!controller)
                    {
                        controller = FindObjectOfType<SquiddyController>();
                    }
                }
            }
            return controller;
        }
    }
    private int usagesLeft;
    private SquiddyController controller;
    private bool firstTime;
    private void OnValidate()
    {
        SetMaxUsages(MaxUsages);
        if (!Root)
        {
            Root = transform.root.gameObject;
        }
        if (!Skill)
        {
            Skill = GetComponent<Skill>();
            if (!Skill)
            {
                Skill = transform.root.GetComponentInChildren<Skill>(true);
            }
        }
    }
    private void Awake()
    {
        SetMaxUsages(MaxUsages);
    }
    public void SetMaxUsages(int maxUsages)
    {
        MaxUsages = Mathf.Max(maxUsages, 1);
        usagesLeft = MaxUsages;
    }
    public void DecrementUsages()
    {
        usagesLeft--;
#if UNITY_EDITOR
        if (DebugEnabled)
        {
            Debug.LogFormat("{0} has {1} usages left!", this, usagesLeft);
        }
#endif
        if (usagesLeft <= 0)
        {
            Recycle();
        }
    }
    private void OnEnable()
    {
        SetMaxUsages(MaxUsages);
        firstTime = true;
        Skill.Initialize(Controller);
        Skill.OnSkillDisabled += DecrementUsages;
    }
    void Update()
    {
        if (firstTime)
        {
            Skill.InvokeSkill(ActivateImmediatly);
            firstTime = !firstTime;
        }
        else
        {
            Skill.InvokeSkill();
        }
    }
    private void OnDisable()
    {
        Skill.OnSkillDisabled -= DecrementUsages;
    }
}
