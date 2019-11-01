using Godot;

public class Player : KinematicBody2D
{
	private Direction _direction = Direction.Right;
	const int WALK_SPEED = 200;
	const float GRAVITY = 25000.0f;
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
		_velocity.y -= 6000.0f;
		// TODO: This jump doesn't feel very good. Tweak the math to be better.
	}

	public void _getInput() {
		_velocity = new Vector2();

		if (Input.IsActionPressed("down")) {
			_direction = Direction.Down;
		}

		if (Input.IsActionPressed("up")) {
			_direction = Direction.Up;

			if (IsGrounded) {
				_jump();
			}
		}

		if (Input.IsActionPressed("right")) {
			_velocity.x = WALK_SPEED;
			_direction = Direction.Right;
		} else if (Input.IsActionPressed("left")) {
			_velocity.x = -WALK_SPEED;
			_direction = Direction.Left;
		} else {
			_velocity.x = 0;
		}
	}

	private void _animate() {
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
		_velocity.y += delta * GRAVITY;
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
		_animPlayer = GetNode<AnimationPlayer>("animPlayer");
		_sprite = GetNode<Sprite>("Sprite");
		_ray = GetNode<RayCast2D>("RayCast2D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
     	_getInput();
		_runGravity(delta);
     	_velocity = MoveAndSlide(_velocity); 
		_animate();
	}
}