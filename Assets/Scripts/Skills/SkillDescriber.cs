using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Skills/Describer")]
public class SkillDescriber : ScriptableObject
{
    public string Description;
    public Sprite Image;
    public Color Color = Color.green;

}