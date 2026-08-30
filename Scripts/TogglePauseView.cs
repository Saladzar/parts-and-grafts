using Godot;
using System;
using Microsoft.VisualBasic;

public partial class TogglePauseView : CanvasLayer 
{
	[Export]bool viewOnPause = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PauseScreen.Instance.GamePauseToggle += ToggleVisibility;
		Hide();
	}

	private void ToggleVisibility(bool isPaused)
	{
			viewOnPause = isPaused;
			Visible=isPaused;
	}
}
