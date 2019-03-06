using UnityEngine;
public abstract class PowerUpLogic : ScriptableObject
{
    [Tooltip("Describer of this power up")]
    public BaseDescriber Describer;
    //particle to instantiate after squiddy take the pu
    public BasicSOPool ParticleAfterTriggerPrefab;


    //audio of trigger
    [Tooltip("Audio event played when pow up is collected")]
    public AudioEvent TriggerSound;
    //audio of vocal saying something when squiddy pick up a power up
    [Tooltip("Audio event played when pow up is collected")]
    public AudioEvent VocalSound;

    [Tooltip("Pool used to spawn rendering object for this power up")]
    public BasicSOPool RenderingObjectPrefab;

    public PowLogicEvent OnCollected;

    public GameObject SpawnPrefab()
    {
        return SpawnPrefab(null);
    }
    public GameObject SpawnPrefab(Transform parent)
    {
        if (RenderingObjectPrefab == null)
        {
            return null;
        }

        int nullObj;
        GameObject go = RenderingObjectPrefab.Get(parent, out nullObj, true);
        go.transform.localPosition = Vector3.zero;
        return go;
    }
    public virtual void PowerUpCollected(Collider player, PowerUp powUp)
    {
        if (OnCollected)
        {
            OnCollected.CurrentLogic = this;
            OnCollected.Raise();
        }
    }
    public virtual void ResetPowerup(GameObject instantiatedPowerUp = null)
    {
        if (instantiatedPowerUp)
        {
            RenderingObjectPrefab.Recycle(instantiatedPowerUp);
        }
    }
    public virtual GameObject InitPowerup(PowerUp powUp)
    {
        return SpawnPrefab(powUp.transform);
    }
}