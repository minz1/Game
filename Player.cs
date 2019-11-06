using Godot;

public class Player : KinematicBody2D
{
    // different constants used for movement math
    private const int WalkSpeed = 300;
    private const int SprintSpeed = 500;
    private const float Gravity = 9000.0f;
    private const float MaxJumpHeight = 1400.0f;

    private int CurrentHealth = 69;
    private int MaximumHealth = 100;

    private Direction PDirection = Direction.Right;
    private PlayerMovementState PMovementState = PlayerMovementState.Still;

    private Vector2 Velocity = new Vector2();
    private AnimationPlayer AnimPlayer;
    private Sprite Sprite;

    /** the purpose of this raycast is to avoid the buggy "IsOnFloor" method, which
	doesn't work well enough for our purposes. this raycast is pointing down just below
	the player character's sprite, which is enough to check if it's touching the ground
	*/
    private RayCast2D GroundRay;

    private RayCast2D InteractRay;

    public Direction Direction
    {
        get { return PDirection; }
    }

    public PlayerMovementState MovementState
    {
        get { return PMovementState; }
    }

    public int Health
    {
        get { return CurrentHealth; }
    }

    public int MaxHealth
    {
        get { return MaximumHealth; }
    }

    public bool IsGrounded()
    {
        /* as explained above, the ray is right below the player, so it's a good way
        of making sure we are truly touching the ground
        */
        return GroundRay.IsColliding();
    }

    /* in godot, a negative velocity means going UP
	 */
    private void Jump()
    {
        Velocity.y -= MaxJumpHeight;
    }

    /* The way this function works is that if the velocity is still going upwards, we remove
	2/5 of the motion so that the player can effectively control the jump height
	 */
    private void JumpCut()
    {
        if (Velocity.y < 0)
        {
            Velocity.y = (Velocity.y * 0.6f);
        }
    }

    // this method is run whenever a player presses the interr=act key
    private void Interact()
    {
        // if our ray is touching an object on the interactable layer, proceed
        if (InteractRay.IsColliding())
        {
            // this is object that we're colliding with
            object collider = InteractRay.GetCollider();

            // if our object implements the Interactable interface, then continue
            if (collider is IInteractable)
            {
                // casts to an interactable interface
                IInteractable interactableObject = (IInteractable)collider;

                // runs the OnInteract method for whatever the player is near
                interactableObject.OnInteract();
            }
        }
    }

    /* this function is being run as a loop every frame. we need the controls to be
	responsive, which is why it is in a loop. this controls the handling for the player's
	direction and running/running state
	 */
    private void RunPlayerControls()
    {
        int currentSpeed = WalkSpeed;

        if (Input.IsActionPressed("sprint"))
        {
            currentSpeed = SprintSpeed;
            PMovementState = PlayerMovementState.Sprinting;
        }
        else
        {
            PMovementState = PlayerMovementState.Walking;
        }

        if (Input.IsActionPressed("up"))
        {
            PDirection = Direction.Up;
        }

        if (Input.IsActionPressed("down"))
        {
            PDirection = Direction.Down;
        }

        if (Input.IsActionPressed("right"))
        {
            PDirection = Direction.Right;
            Velocity.x = currentSpeed;
        }
        else if (Input.IsActionPressed("left"))
        {
            PDirection = Direction.Left;
            Velocity.x = -currentSpeed;
        }
        else
        {
            Velocity.x = 0;
            PMovementState = PlayerMovementState.Still;
        }
    }

    /* this method controls the animations
	When moving, the animations are played for each direction.
	When not moving, the animation player is stopped and the sprite
	is set to the specific frame of our animation that idle is.
	 */
    private void RunAnimations()
    {
        bool isMoving = (MovementState == PlayerMovementState.Walking) || (MovementState == PlayerMovementState.Sprinting);

        if (isMoving)
        {
            switch (PDirection)
            {
                case Direction.Up:
                    AnimPlayer.Play("walkUp");
                    break;
                case Direction.Down:
                    AnimPlayer.Play("walkDown");
                    break;
                case Direction.Left:
                    AnimPlayer.Play("walkLeft");
                    break;
                case Direction.Right:
                    AnimPlayer.Play("walkRight");
                    break;
            }
        }
        else
        {
            AnimPlayer.Stop();

            switch (PDirection)
            {
                case Direction.Up:
                    Sprite.Frame = 16;
                    break;
                case Direction.Down:
                    Sprite.Frame = 25;
                    break;
                case Direction.Left:
                    Sprite.Frame = 1;
                    break;
                case Direction.Right:
                    Sprite.Frame = 12;
                    break;
            }
        }
    }

    /* runs the math for falling.
	*/
    private void RunGravity(float delta)
    {
        if (Velocity.y >= 0)
        {
            // applies gravity constant to our y velocity
            Velocity.y += delta * Gravity;
        }
        else
        {
            // in platformers, when you jump, gravity often doesn't
            // take as much of an effect on the rise, but comes down harder
            // on the fall.
            Velocity.y += delta * Gravity * 0.55f;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // gets our child nodes all loaded up for use
        AnimPlayer = GetNode<AnimationPlayer>("animPlayer");
        Sprite = GetNode<Sprite>("Sprite");
        GroundRay = GetNode<RayCast2D>("GroundRCast");
        InteractRay = GetNode<RayCast2D>("InteractCast");
    }

    public override void _PhysicsProcess(float delta)
    {
        // runs the player control handler method on a loop
        RunPlayerControls();

        // only runs gravity if we're on the ground.
        if (!IsGrounded())
        {
            RunGravity(delta);
        }

        // method used to actually apply this velocity to our player.
        Velocity = MoveAndSlide(Velocity);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // runs the animation loop
        RunAnimations();
    }

    // Input handler method.
    // we are currently using this for the jump event, as we do not want the player
    // to be able to hold down space and jump over and over*/
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("jump"))
        {
            // makes sure we are on the ground before we jump
            if (IsGrounded())
            {
                Jump();
            }
        }

        // makes sure we are off the ground before even considering lowering the jump height
        if (!IsGrounded())
        {
            // runs the jumpCut method if we are off the ground and let go of space bar
            if (@event.IsActionReleased("jump"))
            {
                JumpCut();
            }
        }

        // run the interact method if the interact key is pressed
        if (@event.IsActionPressed("interact"))
        {
            Interact();
        }
    }
}