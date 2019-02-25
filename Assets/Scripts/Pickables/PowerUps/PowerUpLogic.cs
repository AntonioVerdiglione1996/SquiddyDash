using UnityEngine;
public abstract class PowerUpLogic : ScriptableObject
{
    [Tooltip("Describer of this power up")]
    public Describer Describer;
    //particle to instantiate after squiddy take the pu
    public SOPool ParticleAfterTriggerPool;

    //audio of trigger
    [Tooltip("Audio event played when pow up is collected")]
    public AudioEvent TriggerSound;
    //audio of vocal saying something when squiddy pick up a power up
    [Tooltip("Audio event played when pow up is collected")]
    public AudioEvent VocalSound;

    [SerializeField]
    [Tooltip("Pool used to spawn rendering object for this power up")]
    private SOPool renderingObjectPool;

    public GameObject SpawnPrefab()
    {
        return SpawnPrefab(null);
    }
    public GameObject SpawnPrefab(Transform parent)
    {
        if (!renderingObjectPool)
        {
            return null;
        }

        int nullObj;
        GameObject go = renderingObjectPool.Pool.Get(parent, out nullObj, true);
        go.transform.localPosition = Vector3.zero;
        return go;
    }
    public abstract void PowerUpCollected(Collider player, PowerUp powUp);
    public virtual void ResetPowerup(GameObject instantiatedPowerUp = null)
    {
        if (instantiatedPowerUp)
        {
            renderingObjectPool.Pool.Recycle(instantiatedPowerUp);
        }
    }
    public virtual GameObject InitPowerup(PowerUp powUp)
    {
        return SpawnPrefab(powUp.transform);
    }
}