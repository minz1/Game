using Godot;

public class Game : Node2D
{
    private Label _label;
    private Player _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _label = GetNode<Label>("DirectionLabel");
        _player = GetNode<Player>("Player");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        switch(_player.PlayerDirection) {
            case Direction.Up:
                _label.SetText("UP");
                break;
            case Direction.Down:
                _label.SetText("DOWN");
                break;
            case Direction.Left:
                _label.SetText("LEFT");
                break;
            case Direction.Right:
                _label.SetText("RIGHT");
                break;
        }
    }
}