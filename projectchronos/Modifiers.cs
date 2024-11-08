using Godot;
using System;

public partial class Modifiers : CanvasLayer
{

    public override void _Ready()
    {
        base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.WhenPaused;
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
