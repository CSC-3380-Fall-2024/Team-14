using Godot;
using System;

public partial class ProgressBarGeneric : ProgressBar
{
	private bool visible;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		visible = false;
	}
}
