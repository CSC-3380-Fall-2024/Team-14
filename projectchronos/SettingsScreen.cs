using Godot;
using System;
using System.Runtime.CompilerServices;

// silly little enum for silly little screen ratios
public enum AspectRatio {
	SixteenNine,
	FourThree
}

public partial class SettingsScreen : Control {
	// Local variables store option selections
	private bool isHardMode;
	private AspectRatio ratio;

	// we will need references to some specific child nodes (these display text)
	private Label normalLabel;
	private Label hardLabel;
	private Label labelSixteenNine;
	private Label labelFourThree;

	public override void _Ready() {
		isHardMode = false; // normal mode by default
		ratio = AspectRatio.SixteenNine; // who even uses 4:3?

		// acquire the specific nodes for the option text labels
		normalLabel = GetNode<Label>("SettingsOptions/GAMEPLAY SETTINGS/NormalLabel");
		hardLabel = GetNode<Label>("SettingsOptions/GAMEPLAY SETTINGS/HardLabel");
		labelSixteenNine = GetNode<Label>("SettingsOptions/GENERAL SETTINGS/LabelSixteenNine");
		labelFourThree = GetNode<Label>("SettingsOptions/GENERAL SETTINGS/LabelFourThree");
	}

	public bool IsHardMode() {
		return isHardMode;
	}

	// from difficulty selection connected in Godot
	// difficulty button ON is hard mode
	private void OnDifficultyButtonToggled(bool toggled) {
		isHardMode = toggled;
		normalLabel.Visible = !toggled;
		hardLabel.Visible = toggled;
	}

	// from aspect ratio selection, connected in Godot
	// button toggled ON is 4:3, default (off) 16:9
	private void OnAspectRatioButtonToggled(bool toggled) {
		if (toggled) {
			ratio = AspectRatio.FourThree;
		} else {
			ratio = AspectRatio.SixteenNine;
		}

		labelSixteenNine.Visible = !toggled;
		labelFourThree.Visible = toggled;
	}

	// Signal handler method for continue button
	private void OnContinueButtonPressed() {
		Visible = false;
	}

	// Signal handler method for quit to title button
	private void OnQuitToTitleButtonPressed() {
		GetTree().ChangeSceneToFile("res://title_screen.tscn"); // Go to title screen scene
	}
}