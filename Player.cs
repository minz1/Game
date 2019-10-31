using Godot;

public class Player : KinematicBody2D
{
	private Direction _direction = Direction.Right;
	[Export] public int Speed = 200;
	Vector2 _velocity = new Vector2();
	private AnimationPlayer _animPlayer;
	private Sprite _sprite;

	public Direction PlayerDirection {
		get {
			return _direction;
		}
	}

	public void GetInput() {
		_velocity = new Vector2();

		if (Input.IsActionPressed("right")) {
			_velocity.x += 1;
			_direction = Direction.Right;
		}

		if (Input.IsActionPressed("left")) {
			_velocity.x -= 1;
			_direction = Direction.Left;
		}

		if (Input.IsActionPressed("down")) {
			_velocity.y += 1;
			_direction = Direction.Down;
		}

		if (Input.IsActionPressed("up")) {
			_velocity.y -= 1;
			_direction = Direction.Up;
		}

		_velocity = _velocity.Normalized() * Speed;
	}

	private void _animate() {
		if ((_velocity.x == 0) && (_velocity.y == 0)) {
			_animPlayer.Stop();

			switch(PlayerDirection) {
				case Direction.Up:
					_sprite.Frame = 16;
					break;
				case Direction.Down:
					_sprite.Frame = 25;
					break;
				case Direction.Left:
					_sprite.Frame = 1;
					break;
				case Direction.Right:
					_sprite.Frame = 12;
					break;
			}
		} else {
			switch(_direction) {
				case Direction.Up:
					_animPlayer.Play("walkUp");
					break;
				case Direction.Down:
					_animPlayer.Play("walkDown");
					break;
				case Direction.Left:
					_animPlayer.Play("walkLeft");
					break;
				case Direction.Right:
					_animPlayer.Play("walkRight");
					break;
			}
		}
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
		_animPlayer = GetNode<AnimationPlayer>("animPlayer");
		_sprite = GetNode<Sprite>("Sprite");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
     	GetInput();
     	_velocity = MoveAndSlide(_velocity); 
		_animate();
	}
}