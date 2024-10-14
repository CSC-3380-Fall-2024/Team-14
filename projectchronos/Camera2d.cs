using Godot;
using System;

public partial class Camera2d : Camera2D {
	private Vector2 ScreenSize;

	CharacterBody2D player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		player = GetNode<CharacterBody2D>(new NodePath("../Player"));
		ScreenSize = GetViewportRect().Size;
		Position = player.Position;
	}
	
	// The multiple of the screen height difference in camera position
	// that should be allowed before moving the camera
	const float MAX_SCREEN_Y_DIF = 0.25f;
	// The multiple of screen height that should be used when correcting the camera position
	// when the player is on the ground
	private const float CORRECTION_PER_SECOND = 0.005f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		Vector2 pos = Position;
		pos.X = player.Position.X;

		float yDif = Math.Abs(player.Position.Y - Position.Y);
		float yDifDir = Math.Sign(player.Position.Y - Position.Y);
		// Make the smooth movement happen if the player is on the floor or the player is moving quickly away
		if (yDif > 1e-6f) {
			if (yDif > ScreenSize.Y * MAX_SCREEN_Y_DIF) {
				pos.Y += yDifDir * (yDif - ScreenSize.Y * MAX_SCREEN_Y_DIF);
			}
			else if (player.IsOnFloor()) {
				float correction = yDif * ScreenSize.Y * CORRECTION_PER_SECOND * (float)delta * yDifDir;
				if (yDif < Math.Abs(correction)) pos.Y = player.Position.Y;
				else pos.Y += correction;
			}
		}

		Position = pos;
	}
}
