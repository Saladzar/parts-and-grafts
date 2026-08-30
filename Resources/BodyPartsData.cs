using Godot;

[GlobalClass]
public partial class BodyPartsData : Resource
{
    [Export]
    public Texture2D Texture;

    [Export]
    public Shape2D Shape;
}