using Godot;
using System;

public partial class GameManager : Node2D
{
	private CanvasLayer HUDLayer;
	private int gameTime = 0, score = 0;

	public override void _Ready()
	{
		HUDLayer = GetNodeOrNull<CanvasLayer>("HUDLayer");
		if (HUDLayer == null)
		{
			GD.PrintErr("HUDLayer not found in GameManager.");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HUDLayer?.GetNodeOrNull<Label>("GameTime")?.SetText(gameTime.ToString());
	}

	void _on_game_timer_timeout()
	{
		gameTime++;
	}

	public void AddScore(int points)
	{
		score += points;
		HUDLayer?.GetNodeOrNull<Label>("ScoreCounter")?.SetText(score.ToString());
	}
}
