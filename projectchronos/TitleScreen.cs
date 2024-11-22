using Godot;
using System;

// silly little enum for silly little screen ratios
public enum AspectRatio {
	SixteenNine,
	FourThree
}

public partial class TitleScreen : Control {
	private bool isHardMode;
	private AspectRatio screenRatio;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()  {
		ProcessMode = ProcessModeEnum.Always; // makes it so that title screen works when paused;
		isHardMode = false; // normal mode by default
		screenRatio = AspectRatio.SixteenNine; // who even uses 4:3?
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)  {
	}

	// Defining the signal handler method for the start button to be connected via the Godot editor
	private void OnStartButtonPressed() {
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	// Defining the signal handler method for the quit button to be connected via the Godot editor

	private void OnQuitButtonPressed() {
		GetTree().Quit();
	}

	// Defining the signal handler method for the settings button to be connected via the Godot editor
	private void OnSettingsButtonPressed() {
		SettingsScreen settings = GetNode<SettingsScreen>("SettingsScreen");
		settings.Visible = true;
		
	}

	public bool GetDifficulty() {
		return isHardMode;
	}

	public void SetDifficulty(bool isHardMode) {
		this.isHardMode = isHardMode;
	}

	public AspectRatio GetScreenRatio() {
		return screenRatio;
	}

	public void SetAspectRatio(AspectRatio screenRatio) {
		this.screenRatio = screenRatio;
	}
}
