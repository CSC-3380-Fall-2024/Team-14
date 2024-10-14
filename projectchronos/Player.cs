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
			Kill_Reset();
		}

		Velocity = velocity;
		Show();
		MoveAndSlide();
	}

	// Kills player and places them back at start
	//DISCLAIMER removing test code actually breaks the function please do not edit this section unless you are SURE 
	public void Kill_Reset() //DO NOT REMOVE TEST CODE IT BREAKS THINGS :(
	{
		// make dead and move back to starting position
		Hide();
		Position = new Godot.Vector2(0,0);
		Velocity = new Vector2();

		//find the parent node 
		//DO NOT REMOVE TEST CODE  OR EDIT SECTION IT BREAKS FOR SOME REASON
		var currnetNode = GetParent();
		while (currnetNode != null){
			//GD.Print("current" + currnetNode.Name); *Test Code*
			currnetNode = currnetNode.GetParent();
		}
		
		//try to find main
		//DO NOT REMOVE TEST CODE OR EDIT IT BREAKS CODE
		Node currentParent = GetParent();
		while(currentParent!=null){
			//GD.Print("checking node" + currentParent.Name); *Test Code*
			if (currentParent is Main mainNode){
				//GD.Print("main found calling showexit"); *TestCode*
				mainNode.ShowExit();
				return;
			}
			currentParent = currentParent.GetParent();
		}
		
		//GD.Print("main find failed"); *TestCode*
		
	
	}

	
	public void Start(Godot.Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
