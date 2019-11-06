using Godot;

public class Sign : StaticBody2D, IInteractable
{
    private DialogueBox DBox;
    
    public void OnInteract() {
        DBox.DisplayText("Hello, world! Finally got these signs working. Thank GOD.", true);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DBox = GetNode<DialogueBox>("../CanvasLayer/GUI/DialogueBox");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
