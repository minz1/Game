using Godot;
using System;

public class Game : Node2D
{
    private HealthBar HPBar;
    private Player ThePlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HPBar = GetNode<HealthBar>("CanvasLayer/GUI/Bars/HealthBar");
        ThePlayer = GetNode<Player>("Player");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (HPBar.HP != ThePlayer.Health) {
            HPBar.HP = ThePlayer.Health;
        }
    }
}