using UnityEngine;
public abstract class Upgrade : ScriptableObject, IPurchaseObject
{
    [SerializeField]
    private Purchaseable purchaseInfo = new Purchaseable();
    public IPurchaseable PurchaseInfo { get { return purchaseInfo; } }
    public IDescriber Describer { get { return PurchaseInfo.Describer; } }
    public bool IsPurchased { get { return PurchaseInfo.IsPurchased; } set { PurchaseInfo.IsPurchased = value; } }

    public bool OverrideSkill = false;
    public bool OverridePowerup = false;

    public abstract void PowerUpCollected(Collider player, PowerUp powUp, PowerUpLogic logic);
    public abstract void ResetPowerup(PowerUp powUp, PowerUpLogic logic);
    public abstract void InitPowerup(PowerUp powUp, PowerUpLogic logic);

    public abstract void SkillStart(Skill skill);
    public abstract void SkillStop(Skill skill);
    public abstract void SkillUpdate(Skill skill);

    protected virtual void OnValidate()
    {
        if(PurchaseInfo.Describer.Name == null || PurchaseInfo.Describer.Name.Length <= 0)
        {
            PurchaseInfo.Describer.Name = this.name;
        }
    }
}