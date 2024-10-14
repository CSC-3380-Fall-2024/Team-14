using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D {
	// how fast the player moves in pixels/second
	[Export]
	public int speed {get; set;} = 760;

	// gravity is player-specific not world-defined
	public static int gravity = 4000;

	// gravity * 2 is a temporary magic number, can set this value arbitrarily
	public static int fallSpeed = gravity * 2;

	// jump values are set based on desired height
	public static int jumpHeight = 250; // keep this value

	// size of the game window
	public Godot.Vector2 ScreenSize;

	//life limit & bool determinining if we are currentl reseting scene from a kill and a bool showing if we have already done this once per time key is pressed
	private int lives_left;
	private bool reset = false; //checks for being mid reset
	private bool processed = false; //checks for key press action


	// called when the node enters the scene tree for the first time.
	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;
	}

	// we set jump based on desired height, but implement as a velocity delta
	// jumpForce calculation pre-computes velocity delta with gravity
	public int jumpForce = (int) Math.Sqrt(2 * gravity * jumpHeight);

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// what happens while airborne
		if (!IsOnFloor()) {
			
			// keep track of time spent airborne
			if (GetNode<Timer>("FallTimer").IsStopped()) {
				GetNode<Timer>("FallTimer").Start();
			}

			// implement gravity
			velocity += GetGravity() * (float)delta; // positive value because greater Y is lower on the screen
			velocity.Y = Math.Min(velocity.Y, fallSpeed); // clamp vertical fall speed
		} else {
			GetNode<Timer>("FallTimer").Stop();
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
				velocity.Y -= jumpForce; // subtract because screen top is minimum Y coordinate
			}
		}

		// you can hit down key to "cancel" partway through a jump
		if (Input.IsActionPressed("down")) {
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
		if (lives_left > 0){
			lives_left = lives_left -1;
		}

		if(lives_left<=0){
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
		//find main and grab lives from it
		Main mainNode = (Main)GetParent().GetParent();
		lives_left = mainNode.GetMax();

		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// player dies after falling for too long
	private void OnFallTimerTimeout() {
		Kill_Reset();
	}
}