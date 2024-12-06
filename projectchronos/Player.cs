using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Vector2 = Godot.Vector2;

public partial class Player : CharacterBody2D {
	[Export]
	public int speed {get; set;} = 350; // how fast the player moves in pixels/second

	[Export]
	public int gravity = 1960; // gravity is player-specific not world-defined

	[Export]
	public int fallSpeed = 3000;// temporary magic number, can set this value arbitrarily

	[Export]
	public int jumpHeight = 250; // jump values are set based on desired height

	[Export]
	public int PlayerMaxHp = 20;

	private int _playerHp;

	private bool dead = false;
	
	[Export]
	public int PlayerHp {
		get => _playerHp;
		set {
			var healthBar = GetNode<HealthBar>("HealthBar");
			_playerHp = Mathf.Clamp(value, 0, PlayerMaxHp);
			healthBar.MaxValue = PlayerMaxHp;
			healthBar.Value = _playerHp;
			if(_playerHp <= 0 && lives_left >= 0)
			{
				CallDeferred("Kill_Reset");
				
					
			}
		}
	}

	public Godot.Vector2 ScreenSize; // size of the game window

	private bool hasJumpLeft;

	//life limit & bool determining if we are currently resetting scene from a kill and a bool showing if we have already done this once per time key is pressed
	private int lives_left;
	private bool reset = false; //checks for being mid reset
	private bool processed = false; //checks for key press action

	//instantiate animatedSprite2D node for player sprite
	public AnimatedSprite2D playerSprite;

	public int jumpForce;

	//double jump variables
	public int doubleJumpHeight = 20;
	public int doublejumpForce;
	private bool HasDoubJumped;


	// called when the node enters the scene tree for the first time.
	public override void _Ready() {
		ScreenSize = GetViewportRect().Size;

		// we set jump based on desired height, but implement as a velocity delta
		// jumpForce calculation pre-computes velocity delta with gravity

		doublejumpForce = (int) Math.Sqrt(2 * gravity * doubleJumpHeight); //double jump force metrics

		jumpForce = (int) Math.Sqrt(2 * gravity * jumpHeight);

		PlayerHp = PlayerMaxHp;
		var healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Value = PlayerHp;
		healthBar.MaxValue = PlayerHp;

		//get reference to player sprite node
		playerSprite = GetNode<AnimatedSprite2D>("PlayerSprite");

		//play idle animation
		playerSprite.Play("idle");
		//GD.Print("idle in ready");
		Start();
	}

	// called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		UpDirection = Vector2.Up;
		
		Vector2 velocity = Velocity;

		velocity = HandleJumpsAndGravity(velocity, (float) delta);
		

		// read and execute player movement input
		velocity = PlayerControl(velocity, hasJumpLeft || IsOnFloor());
		
		ProcessFire(delta);

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

		if (Input.IsActionPressed("click")) {
			GetChild<PlayerAttack>(6).Attack();
		}
		
		if (Input.IsActionPressed("move_right")) {
			velocity -= horizontalDir * velocity.Dot(horizontalDir);
			velocity += horizontalDir * speed;

			playerSprite.Scale = new Vector2(1, 1);
			if (IsOnFloor()){
				playerSprite.Play("walking");
				//GD.Print("walking right");
			}
			

		} 
		if (Input.IsActionPressed("move_left")) {
			velocity -= horizontalDir * velocity.Dot(horizontalDir);
			velocity -= horizontalDir * speed;

			playerSprite.Scale = new Vector2(-1, 1);
			
			if (IsOnFloor()) {
				playerSprite.Play("walking");
				//GD.Print("walking left");
			}
			
		} if (Input.IsActionJustPressed("jump")) {
			if (IsOnFloor()) {
				velocity += jumpForce * UpDirection;
				hasJumpLeft = true;
				HasDoubJumped = false;
				playerSprite.Play("jumping");
			}		
			else if (hasJumpLeft && !HasDoubJumped) //double jump implementation
			{
				velocity = Vector2.Zero;
				velocity += jumpForce * UpDirection;
				hasJumpLeft = false;
				HasDoubJumped = true;
				playerSprite.Play("jumping");
			}
		}
		
		else if (IsOnFloor()) {
			if (Input.IsActionJustReleased("move_left") || Input.IsActionJustReleased("move_right")) {
				
				playerSprite.Play("idle");
				//GD.Print("idle next");
		
			}
			
		else if (Velocity.X == 0){
			playerSprite.Play("idle");
			//GD.Print("idle last");
		}
	}
		

		// you can hit down key to "cancel" partway through a jump
		if (Input.IsActionPressed("down")) {
			velocity -= UpDirection * Math.Max(Velocity.Dot(UpDirection), 0);
		}

		//kills player if k is pressed **TESTING PROCESS ONLY** removed 11/23

		//testing health bar depletion
		if(Input.IsActionJustPressed("HealthMinus")) {
			PlayerHp -= 2;
		}

		//testing healing
		if (Input.IsActionJustPressed("HealthPlus"))
		{
			PlayerHp += 2;
		}
		
		var hud = GetNode<HUD>(new NodePath("HUD"));
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
			GetTree().ChangeSceneToFile("res://game_over.tscn"); 
		}

		// make dead and move back to starting position
		else{
			Hide();
			Position = GetParent().GetMeta("respawn_coords", Variant.From(new Vector2(0, 0))).AsVector2();
			Velocity = Vector2.Zero;
			processed = false; //rest key tracking
			reset = true; //reset start
			Show();
			MoveAndSlide();
			reset=false; //reset complete
			
			_playerHp = PlayerMaxHp; 
			var upgrade = GetNode<Modifiers>("/root/Main/CanvasLayer/Modifiers");
			upgrade.Show();
			GetTree().Paused = true;
		}
		
	}
	
	public void Start() {
		// find main and grab lives from it
		Main mainNode;
		Node node = GetTree().GetCurrentScene();
		if (node is Main main) {
			mainNode = main;
			lives_left = mainNode.getConfig().MaxLives;
		}
		
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// player dies after falling for too long
	private void OnFallTimerTimeout() {
		CallDeferred("Kill_Reset");
	}

	private double _sinceLastFireTick = 0;
	private double _fireSecondsRemaining = 0;
	/**
	 * Sets the player to be on fire for this many more seconds.
	 * Note that this is not cumulative. 
	 */
	public void SetFireDuration(double seconds) {
		_fireSecondsRemaining = seconds;
	}

	/**
	 * Decrements the number of seconds that the player is on fire by delta and deals fire damage if
	 * a second has passed.
	 */
	private void ProcessFire(double delta) {
		if (_fireSecondsRemaining > 0) {
			Main mainNode = (Main)GetParent().GetParent();
			CpuParticles2D fireAnimation = GetChild<CpuParticles2D>(3);
			if (_sinceLastFireTick > 1) {
				PlayerHp -= mainNode.getConfig().FireDamagePerSecond;
				_sinceLastFireTick -= 1;
				fireAnimation.Emitting = true;
			}

			_sinceLastFireTick += delta;
			_fireSecondsRemaining -= delta;
			if (_fireSecondsRemaining < 0) {
				_fireSecondsRemaining = 0;
				fireAnimation.Emitting = false;
			}
		}
	}
}
