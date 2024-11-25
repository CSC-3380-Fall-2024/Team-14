using Godot;
using System;

public partial class SisyphusEnemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private Vector2 targetSpot;

	public override void _Ready()
	{
		base._Ready();
		targetSpot = new Vector2(0, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}



		Velocity = velocity;
		MoveAndSlide();
	}
}
