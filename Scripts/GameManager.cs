using Godot;
using System;

public partial class GameManager : Node2D
{
	private CanvasLayer HUDLayer;
	private int gameTime = 0, score = 0;

	public override void _Ready()
	{
		HUDLayer = GetParent().GetNodeOrNull<CanvasLayer>("HUD");
		if (HUDLayer == null)
		{
			GD.PrintErr("HUD not found in GameManager.");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HUDLayer?.GetNodeOrNull<Label>("Control/TopHUD/Time/GameTime")?.SetText(gameTime.ToString());
	}

	void _on_timer_timeout()
	{
		gameTime++;
		GD.Print($"Game Time: {gameTime} seconds");
	}

	public int GetGameTime()
	{
		return gameTime;
	}

	public void SetScore(int points)
	{
		score = points;
		HUDLayer?.GetNodeOrNull<Label>("Control/BotHUD/Score/ScoreCounter")?.SetText(score.ToString());
	}

	public int GetScore()
	{
		return score;
	}
}
