using Godot;

[GlobalClass]
public partial class AnimalsData : Resource
{
    [Export]
    public Texture2D Texture;

    [Export]
    public Shape2D Shape;

    [Export]
    public int Health;

    [Export]
    public int Damage;

    [Export]
    public int Size;
}
