using Godot;
using System;

public partial class Enemy : Node2D
{
	private int maxHealth = 1, currentHealth, damage = 5;
	private const float KnockbackStrength = 30f;
	[Export] public BodyPartsData BodyPartData;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentHealth = maxHealth;

		// add to enemies group so units can find this enemy
		AddToGroup("enemies");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
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
			attacker?.AddBodyPart(BodyPartData.Texture, BodyPartData.Shape);
			QueueFree();
		}
	}
}
