using Godot;

[GlobalClass]
public partial class AnimalsData : Resource
{
    [Export]
    public Texture2D Texture;

    [Export]
    public Shape2D Shape;

    [Export]
    public bool FlipHorizontally;

    [Export]
    public BodyPartsData[] BodyParts;

    [Export]
    public int Health = 3;
}
