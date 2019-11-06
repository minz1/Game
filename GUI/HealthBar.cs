using Godot;

public class HealthBar : Node2D
{
    private TextureProgress HPBar;
    private int PlayerMaxHealth = 100;
    private int PlayerHealth = 0;

    public int HP
    {
        get { return PlayerHealth; }

        set
        {
            if ((value > 0) && (value <= PlayerMaxHealth))
            {
                PlayerHealth = value;
                HPBar.Value = value;
            }
            else
            {
                // YOU FOOL. WHY DID YOU NOT INPUT A PROPER VALUE?
                // FIX YOUR CODE.
                HPBar.Value = 0;
            }
        }
    }

    public void SetMaxHealth(int maxHealth) {
        if (maxHealth > 0)
            PlayerMaxHealth = maxHealth;
            HPBar.MaxValue = maxHealth;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        HPBar = GetNode<TextureProgress>("HPBar");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
