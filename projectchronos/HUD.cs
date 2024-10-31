using Godot;
using System;

public partial class HUD : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void SetLives(int lives) {
		var livesContainer = GetNode<FlowContainer>("./Container/LivesContainer");
		if (livesContainer.GetChildCount() > lives) {
			for (int i = livesContainer.GetChildCount(); i > lives; i--) {
				livesContainer.RemoveChild(livesContainer.GetChildren()[i - 1]);
			}
		} else if (livesContainer.GetChildCount() < lives) {
			for (int i = livesContainer.GetChildCount(); i < lives; i++) {
				livesContainer.AddChild(livesContainer.GetChildren()[0].Duplicate());
			}
		}
	}
}
