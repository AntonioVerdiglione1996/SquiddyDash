using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Accessory : MonoBehaviour , IPurchaseObject
{
    public const string Dirname = "Accessory";

    public Transform Root;
    public Skill Skill;
    public EAccessoryType Type;
    public EAccessoryRarity Rarity
    {
        get
        {
            EAccessoryRarity type = EAccessoryRarity.None;
            if (Skill)
            {
                if (Skill is PassiveSkill)
                {
                    type |= EAccessoryRarity.Special;
                }
                else if (Skill is TimedSkill)
                {
                    type |= EAccessoryRarity.Rare;
                }
                else
                {
                    type |= EAccessoryRarity.Legendary;
                }
            }
            else
            {
                type |= EAccessoryRarity.Common;
            }
            return type;
        }
    }

    [SerializeField]
    private Purchaseable purchaseInfo = new Purchaseable();
    public IPurchaseable PurchaseInfo { get { return purchaseInfo; } }
    public IDescriber Describer { get { return PurchaseInfo.Describer; } }
    public bool IsPurchased { get { return PurchaseInfo.IsPurchased; } set { PurchaseInfo.IsPurchased = value; } }

    public uint MaxCollidersRecommended = 0;
    public uint MaxRigidbodiesRecommended = 0;

    public void SetParent(Transform parent, bool worldPositionStays = false)
    {
        Reset();
        Root.SetParent(parent, worldPositionStays);
#if UNITY_EDITOR
        Debug.LogFormat("{0} of type {1} and rarity {2} has been parented to {3}", this, Type, Rarity, parent);
#endif
    }
    public void SetParent(Transform parent, Vector3 localPosition)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localScale = localScale;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
    }
    public void SetParent(Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
    {
        SetParent(parent);
        Root.localPosition = localPosition;
        Root.localRotation = localRotation;
        Root.localScale = localScale;
    }
    private void Awake()
    {
        Reset();
    }
    private void Reset()
    {
        if(PurchaseInfo == null)
        {
            purchaseInfo = new Purchaseable();
        }
        Utils.Builder.Clear();
        Utils.Builder.AppendFormat("{0}_{1}_{2}{3}", (PurchaseInfo.Describer != null ? PurchaseInfo.Describer.Name : name), Rarity, Type, Utils.JSONExtension);
        PurchaseInfo.Filename = Utils.Builder.ToString(0, Utils.Builder.Length).Replace(", ", "_");
        PurchaseInfo.FileDirPath = Dirname;
        Utils.Builder.Clear();

        if (!Skill)
        {
            Skill = GetComponent<Skill>();
        }
        if (!Root)
        {
            Root = transform;
        }
    }
    private void OnValidate()
    {
        Reset();
#if UNITY_EDITOR
        Collider[] colliders = Root.GetComponentsInChildren<Collider>(true);
        Rigidbody[] bodies = Root.GetComponentsInChildren<Rigidbody>(true);
        if (colliders.Length > MaxCollidersRecommended || bodies.Length > MaxRigidbodiesRecommended)
        {
            Debug.LogErrorFormat("{0} contains {1} colliders and {2} rigidbodies, while the recommended amount is {3} colliders and {4} rigidbodies. This may cause weird physics behaviour!", this, colliders.Length, bodies.Length, MaxCollidersRecommended, MaxRigidbodiesRecommended);
        }
#endif
        PurchaseInfo.SaveToFile();
    }
}
