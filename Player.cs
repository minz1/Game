using Godot;

public class Player : KinematicBody2D
{
	// different constants used for movement math
	private const int WALK_SPEED = 300;
	private const int SPRINT_SPEED = 500;
	private const float GRAVITY = 9000.0f;
	private const float MAX_JUMP_HEIGHT = 1400.0f;

	private int currentHealth;
	private int maxHealth;
	private int currentEnergy;
	private int maxEnergy;

	private Direction _direction = Direction.Right;
	private PlayerMovementState _movementState = PlayerMovementState.Still;

	Vector2 _velocity = new Vector2();
	private AnimationPlayer _animPlayer;
	private Sprite _sprite;

	/** the purpose of this raycast is to avoid the buggy "IsOnFloor" method, which
	doesn't work well enough for our purposes. this raycast is pointing down just below
	the player character's sprite, which is enough to check if it's touching the ground
	*/
	private RayCast2D _ray;

	public Direction Direction {
		get {
			return _direction;
		}
	}

	public PlayerMovementState MovementState {
		get {
			return _movementState;
		}
	}

    public bool IsGrounded()
    {
        /* as explained above, the ray is right below the player, so it's a good way
        of making sure we are truly touching the ground
        */
        return _ray.IsColliding();
    }

    /* in godot, a negative velocity means going UP
	 */
    private void _jump() {
		_velocity.y -= MAX_JUMP_HEIGHT;
	}

	/* The way this function works is that if the velocity is still going upwards, we remove
	2/5 of the motion so that the player can effectively control the jump height
	 */
	private void _jumpCut() {
		if (_velocity.y < 0) {
			_velocity.y = (_velocity.y * 0.6f);
		}
	}

	/* this function is being run as a loop every frame. we need the controls to be
	responsive, which is why it is in a loop. this controls the handling for the player's
	direction and running/running state
	 */
	private void _runPlayerControls() {
		int currentSpeed = WALK_SPEED;

		if (Input.IsActionPressed("sprint")) {
			currentSpeed = SPRINT_SPEED;
			_movementState = PlayerMovementState.Sprinting;
		} else {
			_movementState = PlayerMovementState.Walking;
		}

		if (Input.IsActionPressed("up")) {
			_direction = Direction.Up;
		}
		if (Input.IsActionPressed("down")) {
			_direction = Direction.Down;
		}

		if (Input.IsActionPressed("right")) {
			_direction = Direction.Right;
			_velocity.x = currentSpeed;
		} else if (Input.IsActionPressed("left")) {
			_direction = Direction.Left;
			_velocity.x = -currentSpeed;
		} else {
			_velocity.x = 0;
			_movementState = PlayerMovementState.Still;
		}
	}

	/* this method controls the animations
	When moving, the animations are played for each direction.
	When not moving, the animation player is stopped and the sprite
	is set to the specific frame of our animation that idle is.
	 */
	private void _runAnimations() {
		bool isMoving = (MovementState == PlayerMovementState.Walking) || (MovementState == PlayerMovementState.Sprinting);

        if (isMoving) {
            switch (Direction) {
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

            switch (Direction)
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

	/* runs the math for falling.
	*/
	private void _runGravity(float delta) {
		if (_velocity.y >= 0) {
			// applies gravity constant to our y velocity
			_velocity.y += delta * GRAVITY;
		} else {
			// in platformers, when you jump, gravity often doesn't
			// take as much of an effect on the rise, but comes down harder
			// on the fall.
			_velocity.y += delta * GRAVITY * 0.55f;
		}
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
		// gets our child nodes all loaded up for use
		_animPlayer = GetNode<AnimationPlayer>("animPlayer");
		_sprite = GetNode<Sprite>("Sprite");
		_ray = GetNode<RayCast2D>("RayCast2D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
		// runs the player control handler method on a loop
		_runPlayerControls();

		// only runs gravity if we're on the ground.
		if (!IsGrounded()) {
			_runGravity(delta);
		}

		// method used to actually apply this velocity to our player.
     	_velocity = MoveAndSlide(_velocity);
		 // runs the animation loop
		_runAnimations();
	}

	/* Input handler method.
	we are currently using this for the jump event, as we do not want the player
	to be able to hold down space and jump over and over*/
	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("jump")) {
			// makes sure we are on the ground before we jump
			if (IsGrounded()) {
				_jump();
			}
		}

		// makes sure we are off the ground before even considering lowering the jump height
		if (!IsGrounded()) {
			// runs the jumpCut method if we are off the ground and let go of space bar
			if (@event.IsActionReleased("jump")) {
				_jumpCut();
			}
		}
	}
}