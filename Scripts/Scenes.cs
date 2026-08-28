using Godot;
using System;

public partial class Scenes : Node
{
    public static readonly PackedScene Unit =
        GD.Load<PackedScene>("res://Scenes/unit.tscn");
    public static readonly PackedScene Enemy =
        GD.Load<PackedScene>("res://Scenes/enemy.tscn");
}
