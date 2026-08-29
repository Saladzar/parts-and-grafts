using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node2D
{
	private const float MoveSpeed = 100f;
	private const float DashSpeed = 400f;
	private const float DashDuration = 0.3f;
	private const int AttackDamage = 10;

	private Area2D attackArea;
	private Vector2 movementDirection = Vector2.Right;
	private Vector2 dashDirection = Vector2.Zero;
	private float dashTimer;
	private bool isDashing;
	[Export] private Node2D bodyParts;
	[Export] private Area2D playerBodyArea;
	private RandomNumberGenerator rng = new RandomNumberGenerator();
	private int partCount = 0;

	public override void _Ready()
	{
		attackArea = GetNodeOrNull<Area2D>("Area2D");
		if (bodyParts == null)
			bodyParts = GetNodeOrNull<Node2D>("Body Parts");
		rng.Randomize();
		AddToGroup("player");
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;

		if (isDashing)
		{
			Position += dashDirection * DashSpeed * dt;
			dashTimer -= dt;

			if (dashTimer <= 0f)
			{
				isDashing = false;
				dashDirection = Vector2.Zero;
				return;
			}

			CheckEnemyCollisions();
			return;
		}

		Vector2 inputDirection = Vector2.Zero;

		if (Input.IsActionPressed("Left"))
			inputDirection.X -= 1;
		if (Input.IsActionPressed("Right"))
			inputDirection.X += 1;
		if (Input.IsActionPressed("Up"))
			inputDirection.Y -= 1;
		if (Input.IsActionPressed("Down"))
			inputDirection.Y += 1;

		if (inputDirection.LengthSquared() > 0f)
		{
			movementDirection = inputDirection.Normalized();
			Position += movementDirection * MoveSpeed * dt;
		}

		if (Input.IsActionJustPressed("Attack"))
			Attack();
	}

	private void Attack()
	{
		if (isDashing)
			return;

		Vector2 direction = movementDirection;
		if (direction == Vector2.Zero)
			direction = Vector2.Right;

		dashDirection = direction;
		dashTimer = DashDuration;
		isDashing = true;
		CheckEnemyCollisions();
	}

	private void CheckEnemyCollisions()
	{
		HashSet<Enemy> hitEnemies = new();

		foreach (Area2D overlappingArea in playerBodyArea.GetOverlappingAreas())
		{
			if (overlappingArea.GetParent() is not Enemy enemy)
				continue;

			if (!hitEnemies.Add(enemy))
				continue;

			Vector2 knockbackDirection = dashDirection != Vector2.Zero ? dashDirection : movementDirection;
			enemy.Hit(AttackDamage, knockbackDirection, 30f, this);
			isDashing = false;
			dashDirection = Vector2.Zero;
			dashTimer = 0f;
			return;
		}
	}

	private void CollectAttackAreas(Node node, List<Area2D> areas)
	{
		if (node is Area2D area)
			areas.Add(area);

		foreach (Node child in node.GetChildren())
		{
			CollectAttackAreas(child, areas);
		}
	}

	public void AddBodyPart(Texture2D texture, Shape2D shape)
	{
		if (bodyParts == null)
			bodyParts = GetNodeOrNull<Node2D>("Body Parts");

		if (bodyParts == null)
		{
			GD.PushWarning("Player has no Body Parts container to attach a new body part to.");
			return;
		}

		BodyPart newPart = Scenes.BodyPart.Instantiate<BodyPart>();

		Vector2 averageCenter = FindCenterOfBodyParts();

		float radius = GetMaxBodyPartRadius() + 6.67f;

		do
		{
			float angle = rng.RandfRange(0f, Mathf.Tau);
			float distance = rng.RandfRange(radius * 0.7f, radius);

			newPart.Position = averageCenter + new Vector2(
				Mathf.Cos(angle) * distance,
				Mathf.Sin(angle) * distance
			);
		} while (HasOverlap(newPart.Position, shape));


		bodyParts.AddChild(newPart);
		newPart.Setup(texture, shape);

		partCount++;
	}

	private float GetMaxBodyPartRadius()
	{
		float maxRadius = 0f;

		foreach (CollisionShape2D shape in GetBodyCollisions())
		{
			float distance = shape.GlobalPosition.DistanceTo(Position);
			if (distance > maxRadius)
				maxRadius = distance;
		}

		return maxRadius;
	}

	private bool HasOverlap(Vector2 position, Shape2D shape)
	{
		var spaceState = GetWorld2D().DirectSpaceState;

		var query = new PhysicsShapeQueryParameters2D();
		query.Shape = shape;

		Transform2D testTransform = new Transform2D(0, position);

		query.Transform = testTransform;

		var results = spaceState.IntersectShape(query);

		foreach (var result in results)
		{
			Node collider = result["collider"].AsGodotObject() as Node;

			if (collider != null && (collider.IsInGroup("body_parts") || collider.IsInGroup("player")))
			{
				return true;
			}
		}

		return false;
	}


	private Vector2 FindCenterOfBodyParts()
	{
		Vector2 center = Vector2.Zero;
		int count = 0;

		foreach (CollisionShape2D shape in GetBodyCollisions())
		{
			if (shape == null || shape.Shape == null)
				continue;

			center += bodyParts.ToLocal(shape.GlobalPosition);
			count++;
		}

		return count > 0 ? center / count : Vector2.Zero;
	}

	private List<CollisionShape2D> GetBodyCollisions()
	{
		List<CollisionShape2D> shapes = new();
		CollectCollisionShapes(this, shapes);

		if (bodyParts != null)
			CollectCollisionShapes(bodyParts, shapes);

		return shapes;
	}

	private void CollectCollisionShapes(Node node, List<CollisionShape2D> shapes)
	{
		if (node == null)
			return;

		foreach (Node child in node.GetChildren())
		{
			if (child is CollisionShape2D collisionShape && collisionShape.Shape != null)
				shapes.Add(collisionShape);

			CollectCollisionShapes(child, shapes);
		}
	}
}
