using Godot;

public partial class BodyPart : Node2D
{
    [Export] public Sprite2D Sprite;
    [Export] public CollisionShape2D CollisionShape;

    public override void _Ready()
    {
        AddToGroup("body_parts");
    }

    public void Setup(Texture2D texture, Shape2D shape, float rotation = 0f, bool flipH = false)
    {
        if (Sprite == null)
            Sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
        if (CollisionShape == null)
            CollisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

        if (Sprite != null)
        {
            Sprite.Texture = texture;
            Sprite.FlipH = flipH;
        }
        if (CollisionShape != null)
            CollisionShape.Shape = shape;
        
        Rotation = rotation;
    }
}