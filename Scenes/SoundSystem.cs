using Godot;
using System;

public partial class SoundSystem : Node2D
{
	[Export] private AudioStreamPlayer _audioPlayer;
	[Export] private Timer _timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_timer != null)
		{
			_timer.Timeout += OnTimerTimeout;
		}
		else
		{
			GD.PrintErr("SoundSystem: Timer node not found at 'AudioStreamPlayer/Timer'.");
		}

		if (_audioPlayer != null)
		{
			_audioPlayer.Play();
		}
		else
		{
			GD.PrintErr("SoundSystem: AudioStreamPlayer not found at 'AudioStreamPlayer'.");
		}
	}

	private void OnTimerTimeout()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _ExitTree()
	{
		if (_timer != null)
		{
			_timer.Timeout -= OnTimerTimeout;
		}
	}
}
