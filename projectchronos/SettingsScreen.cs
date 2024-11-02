using Godot;
using System;

public partial class SettingsScreen : Control
{
	//Local variables to store option selections
	private int selectedDifficulty;
	private int selectedAspectRatio;
	public override void _Ready() {
		//Initialize to default values 
		selectedDifficulty = 0; //"EASY"
		selectedAspectRatio = 0; //"16:9"
		ProcessMode = ProcessModeEnum.Always;
	}

	//Signal handler method for difficulty selection, connected in Godot
	private void OnDifficultyButtonItemSelected(int index) {
		selectedDifficulty = index; //Store selection
		GD.Print("Difficulty " + selectedDifficulty + " was selected"); //Print to make sure signal from button is connected
	}

	//Signal handler method for aspect ratio selection, connected in Godot
	private void OnAspectRatioButtonItemSelected(int index) {
		selectedAspectRatio = index; //Store selection
		GD.Print("Aspect ratio " + selectedAspectRatio + " was selected"); //Print to make sure signal from button is connected
	}

	//Signal handler method for continue button
	private void OnContinueButtonPressed() {
		GetTree().ChangeSceneToFile("res://main.tscn"); //Go to main scene
	}

	//Signal handler method for quit to title button
	private void OnQuitToTitleButtonPressed() {
		GetTree().ChangeSceneToFile("res://title_screen.tscn"); //Go to title screen scene
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
