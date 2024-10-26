using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D {
	[Export]
	public int speed {get; set;} = 760; // how fast the player moves in pixels/second

	[Export]
	public int gravity = 4000; // gravity is player-specific not world-defined

	[Export]
	public int fallSpeed = 8000;// temporary magic number, can set this value arbitrarily

	[Export]
	public int jumpHeight = 250; // jump values are set based on desired height

	public Godot.Vector2 ScreenSize; // size of the game window

	private bool hasJumpLeft;

	//life limit & bool determining if we are currently resetting scene from a kill and a bool showing if we have already done this once per time key is pressed
	private int lives_left;
	private bool reset = false; //checks for being mid reset
	private bool processed = false; //checks for key press action

	public int jumpForce;


	// called when the node enters the scene tree for the first time.
	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;

		// we set jump based on desired height, but implement as a velocity delta
		// jumpForce calculation pre-computes velocity delta with gravity
		jumpForce = (int) Math.Sqrt(2 * gravity * jumpHeight);

	}

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		UpDirection = -GetGravity().Normalized();
		
		Vector2 velocity = Velocity;

		velocity = HandleJumpsAndGravity(velocity, (float) delta);
		

		// read and execute player movement input
		velocity = PlayerControl(velocity, hasJumpLeft || IsOnFloor());

		Velocity = velocity;
		Show();
		MoveAndSlide();

	}
	
	/// <summary>
	/// Sets the up direction to gravity, manages the fall timer and jump counter, and applies gravity.
	/// Distance is reduced if gravity takes it above the max fall speed.
	/// Gravity is from GetGravity
	/// </summary>
	/// <param name="velocity">Velocity to use as a starting point</param>
	/// <param name="delta">Number of seconds since last call</param>
	/// <returns>The velocity with gravity added</returns>
	private Vector2 HandleJumpsAndGravity(Vector2 velocity, float delta) {
		
		// what happens while airborne
		if (!IsOnFloor()) {
			// keep track of time spent airborne
			if (GetNode<Timer>("FallTimer").IsStopped()) {
				GetNode<Timer>("FallTimer").Start();
			}

			// implement gravity
			velocity += GetGravity() * delta; // positive value because greater Y is lower on the screen
			// Clamp maximum fall speed
			var gravityComp = velocity.Dot(GetGravity().Normalized());
			if (gravityComp > fallSpeed) {
				velocity -= GetGravity().Normalized() * (fallSpeed / gravityComp);
			}
		} else {
			GetNode<Timer>("FallTimer").Stop();
			hasJumpLeft = true; // reset double-jump ability
		}
		
		return velocity;
	}

	private Vector2 PlayerControl(Vector2 velocity, bool canJump) {
		var horizontalDir = UpDirection.Rotated((float) Math.PI / 2f);
		if (Input.IsActionJustReleased("move_left") || Input.IsActionJustReleased("move_right") || IsOnFloor()) {
			// Remove horizontal movement as the player is not moving
			velocity -= horizontalDir * velocity.Dot(horizontalDir);
		}
		
		if (Input.IsActionPressed("move_right")) {
			velocity -= horizontalDir * velocity.Dot(horizontalDir);
			velocity += horizontalDir * speed;
		}

		if (Input.IsActionPressed("move_left")) {
			velocity -= horizontalDir * velocity.Dot(horizontalDir);
			velocity -= horizontalDir * speed;
		}

		if (Input.IsActionJustPressed("jump")) {
			if (canJump) {
				velocity += jumpForce * UpDirection;
				hasJumpLeft = IsOnFloor();
			}
		}

		// you can hit down key to "cancel" partway through a jump
		if (Input.IsActionPressed("down")) {
			velocity -= UpDirection * Math.Max(Velocity.Dot(UpDirection), 0);
		}

		//kills player if k is pressed **TESTING PROCESS ONLY**
		if (Input.IsActionPressed("k")){
			if (!reset && !processed){
				Kill_Reset();
				processed = true; //sets it so we know k is pressed
			}
		} else{
			processed = false; //reset key press to false
		}
		
		var hud = GetNode<HUD>(new NodePath("../HUD"));
		hud.SetLives(lives_left);
		
		Velocity = velocity;
		Show();
		MoveAndSlide();

		return velocity;
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
		lives_left = mainNode.getConfig().MaxLives;

		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// player dies after falling for too long
	private void OnFallTimerTimeout() {
		Kill_Reset();
	}

}
