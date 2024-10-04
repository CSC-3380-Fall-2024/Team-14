using Godot;
using System;

public partial class Player : Area2D {
	// how fast the player moves in pixels/second
	[Export]
	public int speed {get; set;} = 760;

	// gravity is player-specific not world-defined
	public static int gravity = 1500;

	// we set jump amount based on desired height, not "force"
	public static int jumpHeight = 540;

	// size of the game window
	public Vector2 ScreenSize;

	// called when the node enters the scene tree for the first time.
	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;

		Hide();
	}

	// flag for the player being mid-jump or mid-fall is useful for handling player movement inputs
	public bool isAirborne = true;
		
	// by default, the player is not moving
	public MovementVec velocity = new MovementVec();

	// we want to set parameters for gravitation and a jump height, but we implement a jump as a change in velocity
	// jumpForce calculation figure automatically figures out the correct impulse up front
	public int jumpForce = 2 * (int) Math.Sqrt(gravity * jumpHeight);

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		// there's no reason to worry about gravity if we're not in the air
		if (isAirborne) {

			// add a positive value here because higher position is lower on the screen
			velocity.Y += (int) ((float) gravity * delta);
		}

		// by default the player is not inputting horizontal movement
		velocity.X = 0;

		if (Input.IsActionPressed("move_right")) {
			velocity.X += speed;
		}

		if (Input.IsActionPressed("move_left")) {
			velocity.X -= speed;
		}

		if (Input.IsActionPressed("jump")) {
			if (!isAirborne) {
				// subtract because the top of the screen is minimum coordinate
				velocity.Y -= jumpForce;
				isAirborne = true;
			}
		}

		if (Input.IsActionPressed("down")) {

			// you can hit down key to "cancel" partway through a jump
			if (isAirborne) {
				velocity.Y = Math.Max(velocity.Y, 0);
			}
		}

		// we need to actually apply the calculated velocity, and we also need to keep the player bounded within the screen
		Position += new Vector2(velocity.X, velocity.Y) * (float) delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (Position.Y == ScreenSize.Y) {
			isAirborne = false;
			velocity.Y = 0;
		}

		if (Position.Y == 0) {
			velocity.Y = 0;
		}

		Show();
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}

// this is stupid idk why this is like this
public class MovementVec {
	public int X, Y = 0;
}