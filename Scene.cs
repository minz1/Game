using Godot;

public class Scene : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

public enum Direction {
    Up,
    Down,
    Left,
    Right
};

public enum PlayerMovementState {
    Still,
    Walking,
    Sprinting
};