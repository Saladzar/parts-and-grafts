using Godot;
using System;

public partial class PauseScreen : CanvasLayer
{
    [Export] private Button resumeButton;
    [Export] private Button quitButton;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
	}

	void _on_resume_pressed()
	{
		GetTree().Paused = false;
		Hide();
	}

	void _on_quit_pressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}

	void _on_resume_mouse_entered()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(resumeButton, "scale", 
			new Vector2(1.1f, 1.1f), 0.5f).SetTrans(Tween.TransitionType.Circ);
	}

	void _on_resume_mouse_exited()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(resumeButton, "scale", 
			Vector2.One, 0.5f).SetTrans(Tween.TransitionType.Circ);
	}

	void _on_quit_mouse_entered()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(quitButton, "scale", 
			new Vector2(1.1f, 1.1f), 0.5f).SetTrans(Tween.TransitionType.Circ);
	}

	void _on_quit_mouse_exited()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(quitButton, "scale", 
			Vector2.One, 0.5f).SetTrans(Tween.TransitionType.Circ);
	}
}
