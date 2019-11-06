using Godot;
using System;

public class DialogueBox : Panel
{
    private RichTextLabel TextBox;
    private Timer MessageTimer;
    // this is in seconds, this is the time between each letter being printed.
    private const float TimeBetweenLetters = 0.025f;

    // variables for simulated text typing
    // this variable stores the full string in case we need to
    // cut the slow typing short and continue onwards
    private String SlowText;
    // this is the character array which is used to slowly add each character
    private char[] SlowTextArr;
    // current index of the text. we need it to persist through methods, which is why it is here
    private int CurrentTextIndex = 0;
    // boolean to check if the text is currently being printed out.
    private bool IsWriting = false;

    private void Clear() {
        TextBox.Text = "";
    }

    public void DisplayText(String text, bool slowWrite) {
        if (! Visible)
            {
            // this is the max amount of characters we can show before it looks wack
            if (text.Length < 490)
            {
                Visible = true;

                // this kicks off the slow writing process
                if (slowWrite) {
                    // sets our IsWriting status to true
                    IsWriting = true;
                    // sets our full slow text variable
                    SlowText = text;
                    // creates the array needed to add each character
                    SlowTextArr = SlowText.ToCharArray();
                    // starts the timer, will run every secs that TimeBetweenLetters is
                    MessageTimer.Start();
                }
                else
                {
                    // if slowwrite isn't enabled, just show the text
                    TextBox.Text = text;
                }
            }
            else
            {
                // clear the textbox if nothing is going to be shown
                Clear();
            }
        }
    }

    // once we hit zero, this function is called
    public void _OnTimerTimeout()
    {
        // checks to make sure our index doesn't exceed array bounds
        if (CurrentTextIndex < SlowTextArr.Length) {
            // adds the character to our text
            TextBox.Text += SlowTextArr[CurrentTextIndex++];
        }
        else
        {
            // if we have no more characters to show, stop writing and end the timer
            IsWriting = false;
            MessageTimer.Stop();
            CurrentTextIndex = 0;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // always hidden by default
        Visible = false;
        TextBox = GetNode<RichTextLabel>("RichTextLabel");
        MessageTimer = GetNode<Timer>("Timer");

        MessageTimer.SetWaitTime(TimeBetweenLetters);

        MessageTimer.Connect("timeout", this, nameof(_OnTimerTimeout));
    }

    // after the box appears, the player will click to skip. run this if that happens.
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("click"))
		{
            // check to make sure we're being displayed
			if (Visible)
            {
                // if we're writing, skip to the end and display the entire text.
                if (IsWriting)
                {
                    MessageTimer.Stop();
                    TextBox.Text = SlowText;
                    IsWriting = false;
                    CurrentTextIndex = 0;
                }
                // otherwise, make the box go away.
                else
                {
                    Visible = false;
                    Clear();
                }
            }
        }
    }
}
