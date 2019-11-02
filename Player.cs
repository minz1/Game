using Godot;

public class Player : KinematicBody2D
{
	private Direction _direction = Direction.Right;
	const int WALK_SPEED = 500;
	const float GRAVITY = 9000.0f;
	const float JUMP_HEIGHT = 1400.0f;
	Vector2 _velocity = new Vector2();
	private AnimationPlayer _animPlayer;
	private Sprite _sprite;
	private RayCast2D _ray;

	public Direction PlayerDirection {
		get {
			return _direction;
		}
	}

	public bool IsGrounded {
		get {
			return _ray.IsColliding();
		}
	}

	private void _jump() {
		_velocity.y -= JUMP_HEIGHT;
	}

	private void _jumpCut() {
		if (_velocity.y < 0) {
			_velocity.y -= (_velocity.y * 0.4f);
		}
	}
	private void _runPlayerControls() {
		if (Input.IsActionPressed("down")) {
			_direction = Direction.Down;
		}

		if (Input.IsActionPressed("right")) {
			_direction = Direction.Right;
			_velocity.x = WALK_SPEED;
		} else if (Input.IsActionPressed("left")) {
			_direction = Direction.Left;
			_velocity.x = -WALK_SPEED;
		} else {
			_velocity.x = 0;
		}
	}

	private void _runAnimations() {
        if (Input.IsActionPressed("right") || Input.IsActionPressed("left")) {
            switch (_direction) {
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
        } else {
            _animPlayer.Stop();

            switch (PlayerDirection)
            {
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
        }
    }

	private void _runGravity(float delta) {
		if (IsGrounded) {
			_velocity.y = 0;
		} else {
			if (_velocity.y >= 0) {
				_velocity.y += delta * GRAVITY;
			} else {
				_velocity.y += delta * GRAVITY * 0.55f;
			}
		}
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
		_animPlayer = GetNode<AnimationPlayer>("animPlayer");
		_sprite = GetNode<Sprite>("Sprite");
		_ray = GetNode<RayCast2D>("RayCast2D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
		_runPlayerControls();

		if (! IsGrounded) {
			_runGravity(delta);
		}
     	_velocity = MoveAndSlide(_velocity); 
		_runAnimations();
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("up")) {
			_direction = Direction.Up;

			if (IsGrounded) {
				_jump();
			}
		}

		if (! IsGrounded) {
			if (@event.IsActionReleased("up")) {
				_jumpCut();
			}
		}
	}
}