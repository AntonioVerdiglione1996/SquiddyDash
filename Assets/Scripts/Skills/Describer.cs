using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Describer")]
public class Describer : ScriptableObject
{
    public string Name;
    public string Description = string.Empty;
    public Sprite Image;
    public Color Color = Color.green;
    void OnValidate()
    {
        if(Name != null && Name.Length > 0)
        {
            Name = Name.Replace(' ', '_').Replace(',', '_');
        }
    }
}