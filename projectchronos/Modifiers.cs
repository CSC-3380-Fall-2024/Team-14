using Godot;
using System;

public partial class Modifiers : CanvasLayer
{
	private void OnModOnePressed()
	{
		 GetTree().ChangeSceneToFile("res://main.tscn");
	}
	
	private void OnModTwoPressed()
	{
		 GetTree().ChangeSceneToFile("res://main.tscn");
	}
	
	private void OnModThreePressed()
	{
		 GetTree().ChangeSceneToFile("res://main.tscn");
	
	}
}
