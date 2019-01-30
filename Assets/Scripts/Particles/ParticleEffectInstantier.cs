using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/ParticleInstantier")]
public class ParticleEffectInstantier : ScriptableObject
{
    public GameObject ParticleToInstantiate;
  
    public void PoolParticle(GameObject Go)
    {
        ObjectPooler.Get<Poolable>(ParticleToInstantiate, Go.transform);
    }
}
