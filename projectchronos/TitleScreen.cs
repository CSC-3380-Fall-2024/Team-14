using Godot;
using System;

public partial class TitleScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)  {
	}

	//Defining the signal handler method for the start button to be connected via the Godot editor
	private void OnStartButtonPressed() {
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	//Defining the signal handler method for the quit button to be connected via the Godot editor

	private void OnQuitButtonPressed() {
		GetTree().Quit();
	}

	//Defining the signal handler method for the settings button to be connected via the Godot editor
	private void OnSettingsButtonPressed() {
		GetTree().ChangeSceneToFile("res://settings_screen.tscn");
	}
}
