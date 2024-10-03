using Godot;
using System;

public partial class Player : Area2D
{

	[Export]
	public int speed {get; set;} = 400; // how fast the player moves in pixels/s

	public Vector2 ScreenSize; // size of the game window

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
