using Godot;
using System;
using System.Collections.Generic;

public partial class Menu : Node
{
    [Export] private Button _startButton;
    [Export] private Button _quitButton;
    private ButtonGroup group = new ButtonGroup();
    
    public override void _Ready()
    {
        _startButton.Pressed += OnStart;
        _quitButton.Pressed += OnQuit;
        
       _startButton.MouseEntered+=()=>OnHoverEnter(_startButton);
       _startButton.MouseExited+=()=>OnHoverExit(_startButton);
       _quitButton.MouseEntered+=()=>OnHoverEnter(_quitButton);
       _quitButton.MouseExited+=()=>OnHoverExit(_quitButton);

         group = new ButtonGroup();
        _startButton.ButtonGroup = group;
        _quitButton.ButtonGroup = group;
    }
    private void OnButtonGroupPressed(ButtonGroup button)
    {
        if (button.GetPressedButton().Name == "Start") OnStart();
        if (button.GetPressedButton().Name == "Quit") OnQuit();
    }

    private void OnHoverEnter(Button button)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(button, "scale", 
            new Vector2(1.1f, 1.1f), 1.0f).SetTrans(Tween.TransitionType.Circ);
    }

    private void OnHoverExit(Button button)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(button,"scale", Vector2.One, 1.0f).SetTrans(Tween.TransitionType.Circ);
    }

    private void OnStart()
    {
        GetTree().ChangeSceneToFile("res://Scenes/world.tscn");
    }

    private void OnQuit()
    {
        GetTree().Quit();
    }
}
