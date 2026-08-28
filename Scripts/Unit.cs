using Godot;
using System;

public partial class Unit : Node2D
{
	private int speed = 100;
	private Enemy targetEnemy;
	private Area2D attackArea;
	private int damage = 10, health = 100;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if no explicit target set, try to find one
		if (targetEnemy == null)
			FindNearestEnemy();

		if (targetEnemy == null)
			return;

		// ensure target is still valid
		if (!IsInstanceValid(targetEnemy))
		{
			targetEnemy = null;
			return;
		}

		float distance = Position.DistanceTo(targetEnemy.Position);
		if (distance > 10)
		{
			Vector2 direction = (targetEnemy.Position - Position).Normalized();
			Position += direction * speed * (float)delta;
		}
		else
		{
			_Attack();
		}
	}

	public void SetTarget(Enemy enemy)
	{
		targetEnemy = enemy;
	}

    private void _Attack()
    {
        //play attack animation
		targetEnemy.Hit(damage);
    }

	private void FindNearestEnemy()
	{
		Enemy nearestEnemy = null;
		float nearestDistance = float.MaxValue;

		// search enemies by group — more robust than scanning root children
		foreach (Node node in GetTree().GetNodesInGroup("enemies"))
		{
			if (node is Enemy enemy)
			{
				float distance = Position.DistanceTo(enemy.Position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearestEnemy = enemy;
				}
			}
		}

		targetEnemy = nearestEnemy;
	}
}
