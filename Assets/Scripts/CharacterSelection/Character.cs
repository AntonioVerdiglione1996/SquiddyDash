using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
    public bool DebugActive = true;
    public Describer Describer;

    public BasicEvent StartRotation;
    public BasicEvent StopRotation;
    public BasicEvent ResetRotation;
    public BasicEvent StartLerpRotation;

    public ButtonSkillActivator SkillIconUIPrefab;

    public AccessoryLocator[] Locators = new AccessoryLocator[0];

    [SerializeField]
    private Skill[] Skills = null;
    private List<Skill> updatingSkills = null;

    public float DefaultIconsHeightDistance = 128.0f;

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
        if (rotator && !CurrentPlatformForSquiddy.CurrentPlatform)
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
        if (rotator && !CurrentPlatformForSquiddy.CurrentPlatform)
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
        Skills = transform.root.GetComponentsInChildren<Skill>(true);
        Locators = transform.root.GetComponentsInChildren<AccessoryLocator>(true);
#if UNITY_EDITOR
        if (Locators != null && DebugActive)
        {
            Dictionary<EAccessoryType, uint> appearences = new Dictionary<EAccessoryType, uint>();
            for (int i = 0; i < Locators.Length; i++)
            {
                EAccessoryType type = Locators[i].LocatorType;
                if (appearences.ContainsKey(type))
                {
                    appearences[type]++;
                }
                else
                {
                    appearences.Add(type, 1);
                }
            }
            foreach (var item in appearences)
            {
                if (item.Key == EAccessoryType.None)
                {
                    Debug.LogWarningFormat("{0} found {1} invalid accessory locators! Make sure to mark all accessory locators with a valid type!", this, item.Value);
                }
                else if (item.Value > 1)
                {
                    Debug.LogWarningFormat("{0} found {1} duplicates of {2} accessory locator! only the first one will be considered", this, (item.Value - 1), item.Key);
                }
            }
        }
#endif
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
                        rect.position += new Vector3(0f, rect.rect.height * i, 0f);
                    }
                    else
                    {
                        skillIcon.transform.position += new Vector3(0f, i * DefaultIconsHeightDistance, 0f);
                    }
                    skillIcon.gameObject.SetActive(true);
                    uiSpawnedCount++;
                }
                else if (skill.IsSkillAutoActivating)
                {
                    if (updatingSkills == null)
                    {
                        updatingSkills = new List<Skill>();
                    }
                    updatingSkills.Add(skill);
                }
            }
        }
    }

    private void Update()
    {
        if (updatingSkills != null)
        {
            for (int i = 0; i < updatingSkills.Count; i++)
            {
                updatingSkills[i].InvokeSkill();
            }
        }
    }
}
