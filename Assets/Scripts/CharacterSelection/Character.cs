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
    public EDirectionType UiSpawnDirection = EDirectionType.Up;

    public AccessoryLocator[] Locators = new AccessoryLocator[0];

    [SerializeField]
    private Skill[] Skills = null;
    private List<Skill> updatingSkills = null;

    public float DefaultIconsHeightDistance = 128.0f;
    public float DefaultIconsWidthDistance = 128.0f;

    private SquiddyController controller = null;

    private DynamicRotator rotator;

    private Canvas SkillUIHolder;

    public Transform GetAccessoryTransform(EAccessoryType type)
    {
        if (Locators != null)
        {
            for (int i = 0; i < Locators.Length; i++)
            {
                AccessoryLocator locator = Locators[i];
                if (locator.LocatorType == type)
                {
                    return locator.transform;
                }
            }
        }

        return transform;
    }
    public bool CollectAndSpawnSkills(bool spawnIcons = true)
    {
        Skills = transform.root.GetComponentsInChildren<Skill>(true);
        Accessory[] accessories = transform.root.GetComponentsInChildren<Accessory>(true);
        Skill mainSkill = null;
        if (accessories != null && accessories.Length > 0 && Skills != null)
        {
            for (int i = 0; i < Skills.Length; i++)
            {
                Skill skill = Skills[i];
                if (skill.IsMainSkill)
                {
                    mainSkill = skill;
                    break;
                }
            }

            if (mainSkill)
            {
                for (int i = 0; i < accessories.Length; i++)
                {
                    Accessory accessory = accessories[i];
                    if (accessory.Upgrades != null && accessory.Upgrades.Count > 0)
                    {
                        for (int j = 0; j < accessory.Upgrades.Count; j++)
                        {
                            Upgrade up = accessory.Upgrades[i];
                            if (up.IsSkillUpgradable(mainSkill.GetType(), mainSkill))
                            {
                                mainSkill.Upgrades.Add(up);
                            }
                            else
                            {
                                Skill localSkill = accessory.GetComponent<Skill>();
                                if (!localSkill)
                                {
                                    localSkill = accessory.GetComponentInChildren<Skill>(true);
                                }
                                if (localSkill && up.IsSkillUpgradable(localSkill.GetType(), localSkill))
                                {
                                    localSkill.Upgrades.Add(up);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (spawnIcons)
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
                return false;
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
                return false;
            }

            if (!SkillIconUIPrefab)
            {
#if UNITY_EDITOR
                if (DebugActive)
                {
                    Debug.LogErrorFormat("SkillIconUIPrefab needs to be a valid prefab!!!", this);
                }
#endif
                return false;
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
                            switch (UiSpawnDirection)
                            {
                                case EDirectionType.None:
                                case EDirectionType.Up:
                                    rect.position += new Vector3(0f, rect.rect.height * i, 0f);
                                    break;
                                case EDirectionType.Down:
                                    rect.position -= new Vector3(0f, rect.rect.height * i, 0f);
                                    break;
                                case EDirectionType.Left:
                                    rect.position -= new Vector3(rect.rect.width * i, 0f, 0f);
                                    break;
                                case EDirectionType.Right:
                                    rect.position += new Vector3(rect.rect.width * i, 0f, 0f);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (UiSpawnDirection)
                            {
                                case EDirectionType.None:
                                case EDirectionType.Up:
                                    rect.position += new Vector3(0f, i * DefaultIconsHeightDistance, 0f);
                                    break;
                                case EDirectionType.Down:
                                    rect.position -= new Vector3(0f, i * DefaultIconsHeightDistance, 0f);
                                    break;
                                case EDirectionType.Left:
                                    rect.position -= new Vector3(DefaultIconsWidthDistance * i, 0f, 0f);
                                    break;
                                case EDirectionType.Right:
                                    rect.position += new Vector3(DefaultIconsWidthDistance * i, 0f, 0f);
                                    break;
                                default:
                                    break;
                            }
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
        return true;
    }
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
        CollectAndSpawnSkills(false);
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

    private void Update()
    {
        if (updatingSkills != null)
        {
            for (int i = 0; i < updatingSkills.Count; i++)
            {
                updatingSkills[i].InvokeSkill();
#if UNITY_EDITOR
                if (DebugActive)
                {
                    Debug.LogFormat("{0} is invoking {1} automatically", this, updatingSkills[i]);
                }
#endif
            }
        }
    }
}
