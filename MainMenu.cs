using Godot;
using System;

public partial class MainMenu : Control
{
	
	[Export]
	public string StartScenePath = "";

	
	public override void _Ready()
	{
	
	public void OnPlayButtonPressed()
	{
		
		GetTree().ChangeSceneToFile(StartScenePath);
	}
	
	public void OnQuitButtonPressed()
	{
		
		GetTree().Quit();
	}
}


	
	
	
}
