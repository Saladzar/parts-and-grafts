using Godot;
using System;

public partial class Enemy : Node2D
{
	private int maxHealth = 50, currentHealth, damage = 5;

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

	public void Hit(int damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			QueueFree();
		}
	}
}
