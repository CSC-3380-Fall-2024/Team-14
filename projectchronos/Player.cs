using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public Vector2 ScreenSize;

	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		velocity.X = 0;
		if (Input.IsActionPressed("move_right")) {
			velocity.X = Speed;
		}
		if (Input.IsActionPressed("move_left")) {
			velocity.X -= speed;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
