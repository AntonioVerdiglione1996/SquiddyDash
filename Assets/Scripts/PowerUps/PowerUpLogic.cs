using UnityEngine;
public abstract class PowerUpLogic : ScriptableObject
{
    public SkillDescriber Describer;
    public Material[] Materials;
    public Mesh Mesh;
    //particle to instantiate after squiddy take the pu
    public SOPool ParticleAfterTriggerPool;

    //audio of trigger
    public AudioEvent TriggerSound;
    //audio of vocal saying something when squiddy pick up a power up
    public AudioEvent VocalSound;

    [SerializeField]
    protected GlobalEvents GlobalEvents;

    [SerializeField]
    private SOPool prefabPool;
    private GameObject instanciatedPrefab;

    public GameObject GetInstanciatedPrefab()
    {
        return GetInstanciatedPrefab(null);
    }
    public GameObject GetInstanciatedPrefab(Transform parent)
    {
        if (!prefabPool)
        {
            return null;
        }
        if (!instanciatedPrefab)
        {
            int nullObj;
            instanciatedPrefab = prefabPool.Get(parent, out nullObj, true);
            return instanciatedPrefab;
        }

        GlobalEvents.ParentToTarget(parent, instanciatedPrefab.transform);

        return instanciatedPrefab;
    }
    public abstract void PowerUpCollected(Collider player, PowerUp powUp);
    public virtual void ResetPowerup()
    {
        if (instanciatedPrefab)
        {
            prefabPool.Recycle(instanciatedPrefab);
            instanciatedPrefab = null;
        }
    }
    public virtual void InitPowerup(PowerUp powUp)
    {
        GetInstanciatedPrefab(powUp.transform);
    }
}