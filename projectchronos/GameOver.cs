using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	private void OnRestartPressed()
	{
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	private void OnQuitToTitlePressed()
	{
		GetTree().ChangeSceneToFile("title_screen.tscn");
	}
}
