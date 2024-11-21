using Godot;
using System;

public partial class Camera2d : Camera2D {
	private Vector2 ScreenSize;

	CharacterBody2D player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		player = GetNode<CharacterBody2D>(new NodePath("../../"));
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

		var scene = GetNode<Node>("../../../");
		pos = EnforceViewportBounds(
			pos, 
			ScreenSize,
			(float) scene.GetMeta("bound_top", Variant.From(float.NegativeInfinity)).AsDouble(),
			(float) scene.GetMeta("bound_bottom", Variant.From(float.PositiveInfinity)).AsDouble(),
			(float) scene.GetMeta("bound_left", Variant.From(float.NegativeInfinity)).AsDouble(),
			(float) scene.GetMeta("bound_right", Variant.From(float.PositiveInfinity)).AsDouble()
			);

		Position = pos;
	}

	/// <summary>
	/// Enforces the provided bounds on the viewport of this camera.
	/// Having bounds smaller than screenSize is considered undefined behavior.
	/// </summary>
	/// <param name="pos">The current center position of the camera.</param>
	/// <param name="screenSize">The size of the camera's viewport</param>
	/// <param name="boundTop">The top of the camera should not display beyond this.</param>
	/// <param name="boundBottom">The bottom of the camera should not display beyond this.</param>
	/// <param name="boundLeft">The left of the camera should not display beyond this.</param>
	/// <param name="boundRight">The right of the camera should not display beyond this.</param>
	public Vector2 EnforceViewportBounds(Vector2 pos, Vector2 screenSize, float boundTop, float boundBottom, float boundLeft, float boundRight)
	{
		if (pos.Y - screenSize.Y / 2 < boundTop)
		{
			pos.Y = boundTop + screenSize.Y / 2;
		}
		if (pos.Y + screenSize.Y / 2 > boundBottom)
		{
			pos.Y = boundBottom - screenSize.Y / 2;
		}
		if (pos.X - screenSize.X / 2 < boundLeft)
		{
			pos.X = boundLeft + screenSize.X / 2;
		}
		if (pos.X + screenSize.X / 2 > boundRight)
		{
			pos.X = boundRight - screenSize.X / 2;
		}

		return pos;
	}
}
