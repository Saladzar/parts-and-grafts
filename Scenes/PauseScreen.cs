using Godot;
using System;

enum ScreenState
{
    Resume,
    Paused
}
public partial class PauseScreen : Node
{
    public static PauseScreen Instance { get; private set; }
    ScreenState ScreenState;

    [Export] private Button _button;
    
    [Signal]
    public delegate void GamePauseToggleEventHandler(bool isPaused);
    bool isPaused = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;
        if (_button == null) return;
        _button.Pressed += OnMenu;
        
        _button.MouseEntered+=()=>OnHoverEnter(_button);
        _button.MouseExited+=()=>OnHoverExit(_button);
    }
    
    bool Resume()
    {
        return isPaused = false;
    }
    bool Pause()
    {
        return isPaused = true;
    }
    private void OnHoverEnter(Button button)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(button, "scale", 
            new Vector2(1.1f, 1.1f), 1.0f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }
    private void OnHoverExit(Button button)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(button,"scale",
            Vector2.One, 1.0f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && (eventKey.Keycode == Key.Escape)&& eventKey.Pressed)
        {
            if (ScreenState == ScreenState.Paused)
            {
                isPaused = Resume();
                ScreenState = ScreenState.Resume;
            }
            else
            {
                isPaused = Pause();
                ScreenState = ScreenState.Paused;
            }
        }
        GetTree().Paused = isPaused;
        EmitSignal(SignalName.GamePauseToggle, isPaused);
    }
    private void OnMenu()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}