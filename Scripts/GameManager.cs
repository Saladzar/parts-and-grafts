using Godot;
using System;

public partial class GameManager : Node2D
{
	private CanvasLayer HUDLayer;
	[Export] private CanvasLayer gameOverLayer;
	[Export] private CanvasLayer pauseScreenLayer;
	private int gameTime = 0, score = 0;
	private int lives = 3;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		HUDLayer = GetParent().GetNodeOrNull<CanvasLayer>("HUD");
		if (HUDLayer == null)
		{
			GD.PrintErr("HUD not found in GameManager.");
		}

		if (gameOverLayer != null)
			gameOverLayer.ProcessMode = ProcessModeEnum.WhenPaused;
		if (pauseScreenLayer != null)
			pauseScreenLayer.ProcessMode = ProcessModeEnum.Always;

		gameOverLayer?.Hide();
		pauseScreenLayer?.Hide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Keycode == Key.Escape && keyEvent.Pressed && lives > 0)
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
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

		if (GetTree() != null)
		{
			foreach (GodotObject node in GetTree().GetNodesInGroup("enemies"))
			{
				if (node is Enemy enemy)
					enemy.UpdateLabelColor();
			}
		}
	}

	public int GetScore()
	{
		return score;
	}

	public void LoseLife()
	{
		lives = Mathf.Max(0, lives - 1);
		CheckLives();
		GD.Print($"Player lost a life. Remaining lives: {lives}");

		if (lives <= 0)
		{
			if (GetTree() != null)
				GetTree().Paused = true;
			pauseScreenLayer?.Hide();
			GD.Print("Game Over!");
			gameOverLayer?.Show();
		}
	}

	public void TogglePause()
	{
		if (lives <= 0)
			return;

		bool paused = !GetTree().Paused;
		GetTree().Paused = paused;

		if (paused)
			pauseScreenLayer?.Show();
		else
			pauseScreenLayer?.Hide();
	}

    private void CheckLives()
    {
        switch (lives)
		{
			case 2:
				HUDLayer?.GetNodeOrNull<TextureRect>("Control/TopHUD/Lives/Heart3")?.Hide();
				break;
			case 1:
				HUDLayer?.GetNodeOrNull<TextureRect>("Control/TopHUD/Lives/Heart2")?.Hide();
				break;
			case 0:
				HUDLayer?.GetNodeOrNull<TextureRect>("Control/TopHUD/Lives/Heart1")?.Hide();
				break;
		}
    }

    public int GetLives()
	{
		return lives;
	}
}
