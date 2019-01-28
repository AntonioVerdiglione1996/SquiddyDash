using UnityEngine;
public abstract class PowerUpLogic : ScriptableObject
{
    public SkillDescriber Describer;
    public Material[] Materials;
    public Mesh Mesh;
    //particle to instantiate after squiddy take the pu
    public GameObject ParticleAfterTrigger;

    //audio of trigger
    public AudioEvent TriggerSound;
    //audio of vocal saying something when squiddy pick up a power up
    public AudioEvent VocalSound;

    [SerializeField]
    protected GlobalEvents GlobalEvents;

    [SerializeField]
    private GameObject prefab;
    private GameObject instanciatedPrefab;

    public GameObject GetInstanciatedPrefab()
    {
        if (!prefab)
        {
            return null;
        }
        if (!instanciatedPrefab)
        {
            instanciatedPrefab = Instantiate<GameObject>(prefab);
            return instanciatedPrefab;
        }

        return instanciatedPrefab;
    }
    public GameObject DeactivateAndActivateInstancedPrefab(Transform parent)
    {
        GameObject go = GetInstanciatedPrefab(parent);
        go.SetActive(false);
        go.SetActive(true);
        return go;
    }
    public GameObject DeactivateAndActivateInstancedPrefab()
    {
        GameObject go = GetInstanciatedPrefab();
        go.SetActive(false);
        go.SetActive(true);
        return go;
    }
    public GameObject GetInstanciatedPrefab(Transform parent)
    {
        if (!prefab)
        {
            return null;
        }
        if (!instanciatedPrefab)
        {
            instanciatedPrefab = Instantiate<GameObject>(prefab, parent);
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
            instanciatedPrefab.SetActive(false);
        }
    }
    public virtual void InitPowerup(PowerUp powUp)
    {
        GetInstanciatedPrefab(powUp.transform);
        if (instanciatedPrefab)
        {
            instanciatedPrefab.SetActive(true);
        }
    }
}