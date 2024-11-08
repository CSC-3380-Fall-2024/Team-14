using Godot;
using System;

public partial class Modifiers : CanvasLayer
{

    public override void _Ready()
    {
        base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.Always;
    }
    private void OnModOnePressed()
	{
		 GetTree().ChangeSceneToFile("res://.tscn");
	}
	
	private void OnModTwoPressed()
	{
		 GetTree().ChangeSceneToFile("res://world.tscn");
	}
	
	private void OnModThreePressed()
	{
		 GetTree().ChangeSceneToFile("res://world.tscn");
	
	}
}
