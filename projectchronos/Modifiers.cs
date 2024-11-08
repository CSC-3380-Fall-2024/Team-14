using Godot;
using System;

public partial class Modifiers : Control
{

	public override void _Ready()
	{
		base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.WhenPaused;
	}

    public override void _Process(double delta)
    {
        if (Visible == true)
		{
			GetTree().Paused = true;
		}
    }

    private void OnModOnePressed()
	{
		 Visible = false;
	}
	
	private void OnModTwoPressed()
	{
		 Visible = false;
	}
	
	private void OnModThreePressed()
	{
		 Visible = false;
	}
}
