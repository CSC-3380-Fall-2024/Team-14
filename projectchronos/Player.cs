using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D {
	// how fast the player moves in pixels/second
	[Export]
	public int speed {get; set;} = 760;

	// gravity is player-specific not world-defined
	public static int gravity = 4000;

	// we set jump amount based on desired height, not "force"
	public static int jumpHeight = 250; // keep this value

	// size of the game window
	public Godot.Vector2 ScreenSize;

	//life limit & bool determinining if we are currentl reseting scene from a kill and a bool showing if we have already done this once per time key is pressed
	private int lives = 3;
	private bool reset = false; //checks for being mid reset
	private bool processed = false; //checks for key press action


	// called when the node enters the scene tree for the first time.
	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;

	
	}

	// we want to set parameters for gravitation and a jump height, but we implement a jump as a change in velocity
	// jumpForce calculation figure automatically figures out the correct impulse up front
	public int jumpForce = (int) Math.Sqrt(2 * gravity * jumpHeight);

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// there's no reason to worry about gravity if we're not in the air
		if (!IsOnFloor()) {

			// add a positive value here because higher position is lower on the screen
			velocity += (GetGravity() * (float)delta);
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
			if (IsOnFloor()) {
				// subtract because the top of the screen is minimum coordinate
				velocity.Y -= jumpForce;
			}
		}

		if (Input.IsActionPressed("down")) {

			// you can hit down key to "cancel" partway through a jump
			velocity.Y = Math.Max(velocity.Y, 0);
		}

		//kills player if k is pressed **TESTING PROCESS ONLY**
		if (Input.IsActionPressed("k")){
			if (!reset && !processed){
				Kill_Reset();
				processed = true; //sets it so we know k is pressed
			}
		}

		else{
			processed = false; //reset key press to false
		}

		Velocity = velocity;
		Show();
		MoveAndSlide();
	}

	// Kills player and places them back at start
	public void Kill_Reset() 
	{
		if (lives > 0){
			lives = lives -1;
		}

		if(lives<=0){
			ShowExitScreen();
		}

		// make dead and move back to starting position
		else{
			Hide();
			Position = new Godot.Vector2(0,0);
			Velocity = Vector2.Zero;
			processed = false; //rest key tracking
			reset = true; //reset start
			Show();
			MoveAndSlide();
			reset=false; //reset complete
		}
		
	}

	public void ShowExitScreen() 
	{
		//find the parent node 
		Node currnetNode = GetParent();
		while (currnetNode != null){
			currnetNode = currnetNode.GetParent();
		}
	
		//try to find main
		//runs the show exit code
		Node currentParent = GetParent();
		while(currentParent!=null){
			if (currentParent is Main mainNode){
				mainNode.ShowExit();
				return;
			}
			currentParent = currentParent.GetParent();
		}
	}

	
	public void Start(Godot.Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
