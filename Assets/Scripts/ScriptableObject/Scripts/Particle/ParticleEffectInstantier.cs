using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Particle/Instantier")]
public class ParticleEffectInstantier : ScriptableObject
{
    public GameObject ParticleToInstantiate;
  
    public void PoolParticle(GameObject Go)
    {
        ObjectPooler.Get<Poolable>(ParticleToInstantiate, Go.transform);
    }
}
