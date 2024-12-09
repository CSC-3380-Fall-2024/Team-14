using Godot;
using System;

public partial class TheEnd : CanvasLayer {
	private void OnExitPressed() {
		GetTree().Quit();
	}

	private void OnQuitToTitlePressed() {
		GetTree().ChangeSceneToFile("title_screen.tscn");
	}
}
