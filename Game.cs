using Godot;

public class Game : Node2D
{
    private Label _directionLabel;
    private Label _groundLabel;
    private Player _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _directionLabel = GetNode<Label>("DirectionLabel");
        _groundLabel = GetNode<Label>("GroundLabel");
        _player = GetNode<Player>("Player");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        switch(_player.PlayerDirection) {
            case Direction.Up:
                _directionLabel.SetText("UP");
                break;
            case Direction.Down:
                _directionLabel.SetText("DOWN");
                break;
            case Direction.Left:
                _directionLabel.SetText("LEFT");
                break;
            case Direction.Right:
                _directionLabel.SetText("RIGHT");
                break;
        }
        if (_player.IsGrounded) {
            _groundLabel.SetText("ON FLOOR");
        } else {
            _groundLabel.SetText("IN AIR");
        }
    }
}