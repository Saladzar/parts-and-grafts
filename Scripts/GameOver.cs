using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	void _on_button_pressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
