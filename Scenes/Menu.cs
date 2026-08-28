using Godot;
using System;

public partial class Menu : Node
{
	public override void _Ready()
	{
		Button start = GetNode<Button>("MarginContainer/VBoxContainer/PlayButton");
		Button quit = GetNode<Button>("MarginContainer/VBoxContainer/QuitButton");

		start.Pressed += OnStart;
		quit.Pressed += OnQuit;
	}

	private void OnStart()
	{
		var scene = GD.Load<PackedScene>("res://Scenes/StartGame.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
