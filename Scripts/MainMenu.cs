using Godot;

public partial class MainMenu : Control
{
	
	[Export]
	public string StartScenePath = "res://Scenes/world.tscn";

	
	public override void _Ready()
	{
		
	}

	void _on_play_button_pressed()
	{
		Error result = GetTree().ChangeSceneToFile(StartScenePath);

		GD.PrintErr("ChangeSceneToFile result: " + result);
	}

	void _on_quit_button_pressed()
	{
		GD.PrintErr("Quit button pressed. Exiting the game.");
		GetTree().Quit();
	}
}
