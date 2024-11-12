using Godot;
using System;
using System.ComponentModel.Design;

public partial class Modifiers : Control
{
	

	public override void _Ready()
	{
		base._Ready();
		Visible = false;
		ProcessMode = ProcessModeEnum.Always;
		GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod2").Disabled = true;
		GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod3").Disabled = true;
	}

	public override void _Process(double delta)
	{
		
	}


	private void OnModOnePressed()
	{
		 Visible = false;
		 GetTree().Paused = false;
		 GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod2").Disabled = false;

	}
	
	private void OnModTwoPressed()
	{
		 Visible = false;
		 GetTree().Paused = false;
		 GetNode<Button>("/root/Main/CanvasLayer/Modifiers/HBoxContainer/Mod3").Disabled = false;
	}
	
	private void OnModThreePressed()
	{
		 Visible = false;
		 GetTree().Paused = false;
	}
}
