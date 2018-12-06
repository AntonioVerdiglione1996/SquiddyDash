using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
#if UNITY_EDITOR
    public bool DebugActive = true;
#endif
    public string Name;
    public Color colorName;
    public Sprite Icon;

    public GameEvent StartRotation;
    public GameEvent StopRotation;
    public GameEvent ResetRotation;
    public GameEvent StartLerpRotation;

    public ButtonSkillActivator SkillIconUIPrefab;

    [SerializeField]
    private Skill[] Skills = null;

    private SquiddyController controller = null;

    private DynamicRotator rotator;

    private Canvas SkillUIHolder;

    private void OnEnable()
    {
        if (rotator)
        {
            if (StartRotation)
            {
                StartRotation.OnEventRaised += StartRot;
            }
            if (ResetRotation)
            {
                ResetRotation.OnEventRaised += RestartRot;
            }
            if (StartLerpRotation)
            {
                StartLerpRotation.OnEventRaised += StartLerpRot;
            }
            if (StopRotation)
            {
                StopRotation.OnEventRaised += StopRot;
            }
        }
    }
    private void OnDisable()
    {
        if (rotator)
        {
            if (StartRotation)
            {
                StartRotation.OnEventRaised -= StartRot;
            }
            if (ResetRotation)
            {
                ResetRotation.OnEventRaised -= RestartRot;
            }
            if (StartLerpRotation)
            {
                StartLerpRotation.OnEventRaised -= StartLerpRot;
            }
            if (StopRotation)
            {
                StopRotation.OnEventRaised -= StopRot;
            }
        }
    }
    private void StartRot()
    {
        if (rotator)
        {
            rotator.ChangeCurrentState(DynamicRotatorState.Rotating);
        }
    }
    private void StopRot()
    {
        StartLerpRot();
    }
    private void RestartRot()
    {
        if (rotator)
        {
            rotator.TryCalculateNewRotationValues();
        }
    }
    private void StartLerpRot()
    {
        if (rotator)
        {
            rotator.ChangeCurrentState(DynamicRotatorState.LerpToDefault);
        }
    }
    private void OnValidate()
    {
        Skills = GetComponentsInChildren<Skill>();
    }

    private void Awake()
    {
        rotator = GetComponent<DynamicRotator>();
    }

    private void Start()
    {
        controller = transform.root.GetComponentInChildren<SquiddyController>();
        if (!controller)
        {
#if UNITY_EDITOR
            if (DebugActive)
            {
                Debug.LogWarningFormat("{0} could not find a valid controller reference!", this);
            }
#endif
            return;
        }

        SkillUIHolder = transform.root.GetComponentInChildren<Canvas>();
        if (!SkillUIHolder)
        {
#if UNITY_EDITOR
            if (DebugActive)
            {
                Debug.LogErrorFormat("{0} could not find a valid Canvas reference to hold the skill icons!", this);
            }
#endif
            return;
        }

        if (!SkillIconUIPrefab)
        {
#if UNITY_EDITOR
            if (DebugActive)
            {
                Debug.LogErrorFormat("SkillIconUIPrefab needs to be a valid prefab!!!", this);
            }
#endif
            return;
        }

        if (Skills != null)
        {
            int uiSpawnedCount = 0;
            for (int i = 0; i < Skills.Length; i++)
            {
                Skill skill = Skills[i];
                skill.Initialize(controller);
                if (!skill.IsSkillAutoActivating && SkillIconUIPrefab && SkillUIHolder)
                {
                    ButtonSkillActivator skillIcon = Instantiate<ButtonSkillActivator>(SkillIconUIPrefab, SkillUIHolder.transform);
                    skillIcon.ActivableSkill = skill;
                    RectTransform rect = skillIcon.GetComponent<RectTransform>();
                    if (rect)
                    {
                        rect.position += new Vector3(0f,rect.rect.height * i, 0f);
                    }
                    else
                    {
                        skillIcon.transform.position += new Vector3(0f, i * 128, 0f);
                    }
                    skillIcon.gameObject.SetActive(true);
                    uiSpawnedCount++;
                }
            }
        }
    }

    private void Update()
    {
        if (Skills != null)
        {
            for (int i = 0; i < Skills.Length; i++)
            {
                Skill skill = Skills[i];
                if (skill.IsSkillAutoActivating)
                {
                    skill.InvokeSkill();
                }
            }
        }
    }
}
