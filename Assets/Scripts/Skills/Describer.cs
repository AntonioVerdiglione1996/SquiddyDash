using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Describer")]
public class Describer : ScriptableObject
{
    public string Name;
    public string[] Descriptions = new string[0];
    public Sprite Image;
    public Color Color = Color.green;

}