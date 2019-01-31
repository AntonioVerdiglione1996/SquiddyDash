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
    private SOPool prefabPool;

    public GameObject SpawnPrefab()
    {
        return SpawnPrefab(null);
    }
    public GameObject SpawnPrefab(Transform parent)
    {
        if (!prefabPool)
        {
            return null;
        }

        int nullObj;
        GameObject go = prefabPool.Get(parent, out nullObj, true);
        go.transform.localPosition = Vector3.zero;
        return go;
    }
    public abstract void PowerUpCollected(Collider player, PowerUp powUp);
    public virtual void ResetPowerup(GameObject instantiatedPowerUp = null)
    {
        if (instantiatedPowerUp)
        {
            prefabPool.Recycle(instantiatedPowerUp);
        }
    }
    public virtual GameObject InitPowerup(PowerUp powUp)
    {
        return SpawnPrefab(powUp.transform);
    }
}