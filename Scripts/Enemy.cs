using Godot;
using System;

public partial class Enemy : Node2D
{
	private float speed = 100f, size = 1.0f, scaleDown = 0.35f;
	private int maxHealth, currentHealth, score;
	private const float KnockbackStrength = 30f;
	[Export] public BodyPartsData[] BodyPartData;
	[Export] public AnimalsData AnimalData;
	Random rnd = new Random();

	private Sprite2D sprite;
	private CollisionShape2D collisionShape;
	[Export] private Label sizeDisplay;
	[Export] public float ExtraPadding { get; set; } = 10.0f;

	private Vector2 dynamicOffset = Vector2.Zero;

	private GameManager gameManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		gameManager = GetTree().CurrentScene.GetNodeOrNull<GameManager>("GameManager");

		AddToGroup("enemies");
	}

	public void MakeType(AnimalsData animalData)
	{
		if (animalData == null)
		{
			GD.PrintErr("MakeType called with null AnimalsData.");
			return;
		}

		AnimalData = animalData;
		if (sprite != null)
			sprite.Texture = AnimalData.Texture;
		if (collisionShape != null)
			collisionShape.Shape = AnimalData.Shape;
		BodyPartData = AnimalData.BodyParts;
		maxHealth = animalData.Health;
		currentHealth = maxHealth;
	}

	public void SetSize(float newSize)
	{
		size = newSize;
		Scale = Vector2.One * size * scaleDown;
	}

	public void SetScore(int baseScore)
	{
		score = (int)Math.Round(baseScore * size);
	}

	public void ConfigureLabel()
	{
		float textureHeight = sprite.Texture.GetSize().Y;
		float visualHeight = textureHeight * Scale.Y;
		float halfHeight = visualHeight / 2.0f;

		dynamicOffset = new Vector2(0, -(halfHeight + ExtraPadding));
		sizeDisplay.HorizontalAlignment = HorizontalAlignment.Center;

		sizeDisplay.Text = score.ToString();

		if (score > gameManager.GetScore())
			sizeDisplay.LabelSettings.FontColor = Color.Color8(255, 0, 0);
		else
			sizeDisplay.LabelSettings.FontColor = Color.Color8(0, 255, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction = GetNodeOrNull<Player>("../Player").GetPosition() - Position;
		if (direction != Vector2.Zero)
			Position += direction.Normalized() * speed * (float)delta;

		if (AnimalData.FlipHorizontally)
		{
			if (direction.X < 0)
				sprite.FlipH = true;
			else
				sprite.FlipH = false;
		}
		else
		{
			if (direction.X < 0)
				sprite.FlipH = false;
			else
				sprite.FlipH = true;

		if (sizeDisplay != null)
			{
				// Update the label's position relative to the enemy's global position
				sizeDisplay.GlobalPosition = GlobalPosition + dynamicOffset;

				// Prevent the label from flipping or rotating if the parent does
				sizeDisplay.Rotation = 0;
				sizeDisplay.Scale = Vector2.One;
			}
		}
	}

	public void Hit(int damage, Vector2 knockbackDirection = default, float knockbackStrength = 0f, Player attacker = null)
	{
		currentHealth -= damage;

		if (knockbackStrength > 0f && knockbackDirection != Vector2.Zero)
		{
			Position += knockbackDirection.Normalized() * knockbackStrength;
		}

		if (currentHealth <= 0)
		{
			if (BodyPartData != null && BodyPartData.Length > 0)
				attacker?.AddBodyPart(BodyPartData[rnd.Next(BodyPartData.Length)]);
			QueueFree();
		}
	}
}
