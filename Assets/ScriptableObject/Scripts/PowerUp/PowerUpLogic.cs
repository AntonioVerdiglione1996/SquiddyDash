using UnityEngine;
public abstract class PowerUpLogic : ScriptableObject
{
    public SkillDescriber Describer;
    public Material[] Materials;
    public Mesh Mesh;
    public abstract void PowerUpCollected(Collider player, PowerUp powUp);
}