using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class SettingsScreen : Control {
	// we will need references to some specific child nodes (these display text)
	private Label normalLabel;
	private Label hardLabel;
	private Label labelSixteenNine;
	private Label labelFourThree;

	private TitleScreen titleScreen;

	public override void _Ready() {
		// acquire the specific nodes for the option text labels
		normalLabel = GetNode<Label>("SettingsOptions/GAMEPLAY SETTINGS/NormalLabel");
		hardLabel = GetNode<Label>("SettingsOptions/GAMEPLAY SETTINGS/HardLabel");
		labelSixteenNine = GetNode<Label>("SettingsOptions/GENERAL SETTINGS/LabelSixteenNine");
		labelFourThree = GetNode<Label>("SettingsOptions/GENERAL SETTINGS/LabelFourThree");

		// acquire the title screen node
		titleScreen = GetParent<TitleScreen>();
	}

    public override void _PhysicsProcess(double delta) {
		normalLabel.Visible = !titleScreen.GetDifficulty();
		hardLabel.Visible = titleScreen.GetDifficulty();
		labelFourThree.Visible = titleScreen.GetScreenRatio() == AspectRatio.FourThree;
		labelSixteenNine.Visible = titleScreen.GetScreenRatio() == AspectRatio.SixteenNine;
    }

    // from difficulty selection connected in Godot
    // difficulty button ON is hard mode
    private void OnDifficultyButtonToggled(bool toggled) {
		titleScreen.SetDifficulty(toggled);
	}

	// from aspect ratio selection, connected in Godot
	// button toggled ON is 4:3, default (off) 16:9
	private void OnAspectRatioButtonToggled(bool toggled) {
		if (toggled) {
			titleScreen.SetAspectRatio(AspectRatio.FourThree);
			GetTree().Root.ContentScaleSize = new Vector2I(1920, 1440);
		} else {
			titleScreen.SetAspectRatio(AspectRatio.SixteenNine);
			GetTree().Root.ContentScaleSize = new Vector2I(1920, 1080);
		}
	}

	// Signal handler method for continue button
	private void OnContinueButtonPressed() {
		Visible = false;
	}

	// Signal handler method for quit to title button
	private void OnQuitToTitleButtonPressed() {
		Visible = false; // Go to title screen scene
	}
}