using UnityEngine;
[CreateAssetMenu(menuName = "Gameplay/Describer")]
public class Describer : ScriptableObject, IDescriber
{
    public BaseDescriber Info = new BaseDescriber();

    public string Name
    {
        get { return Info.Name; }
        set { Info.Name = value; }
    }
    public string Description
    {
        get { return Info.Description; }
        set { Info.Description = value; }
    }
    public Sprite Image
    {
        get { return Info.Image; }
        set { Info.Image = value; }
    }
    public Color Color
    {
        get { return Info.Color; }
        set { Info.Color = value; }
    }
    public Material Material
    {
        get { return Info.Material; }
        set { Info.Material = value; }
    }

    public void OnValidate()
    {
        Info.OnValidate();
    }
}
[System.Serializable]
public class BaseDescriber : IDescriber
{
    [SerializeField]
    private string title;
    [SerializeField]
    private string description = string.Empty;
    [SerializeField]
    private Sprite image;
    [SerializeField]
    private Color color = Color.green;
    [SerializeField]
    private Material material;

    public string Name
    {
        get { return title; }
        set { title = value; }
    }
    public string Description
    {
        get { return description; }
        set { description = value; }
    }
    public Sprite Image
    {
        get { return image; }
        set { image = value; }
    }
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }
    public Material Material
    {
        get { return material; }
        set { material = value; }
    }

    public void OnValidate()
    {
        if (Name != null && Name.Length > 0)
        {
            Name = Name.Replace(' ', '_').Replace(',', '_');
        }
    }
}
public interface IDescriber
{
    string Name { get; set; }
    string Description { get; set; }
    Sprite Image { get; set; }
    Color Color { get; set; }
    Material Material { get; set; }
    void OnValidate();
}