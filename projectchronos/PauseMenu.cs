using Godot;
using System;


public partial class PauseMenu : Control
{

	 public override void _Ready()
	{
		//hide menu upon loading up the world
		Hide();
	}
	
	public override void _Process(double delta)
{
	 if (Input.IsActionJustPressed("ui_cancel") && !GetTree().Paused)
		{
			Pause();
		}
		else if (Input.IsActionJustPressed("ui_cancel") && GetTree().Paused)
		{
			Resume();
		}
}

 public void Resume()
	{
		GetTree().Paused = false;
		Hide();
	}

 public void Pause()
	{
		GetTree().Paused = true;
		Show();
	}

 private void OnSettingsPressed()
 {
	GetTree().ChangeSceneToFile("settings_screen.tscn");
 }

 private void OnExitPressed()
 {
	GetTree().ChangeSceneToFile("title_screen.tscn");
 }
}
