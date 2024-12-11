using Godot;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class Fireball : Area2D
{
	public float Speed = 350f; //speed in pix/s
	public int Damage = 7; //damage amt

	public static int DebuffDuration = 5; //how long it inflicts debuffs (on fire here)
	public static int Lifetime = 4; // seconds to despawn

	private Player _player;

	private AnimatedSprite2D fireballSprite;

	private Player player
	{
		get
		{
			if (_player == null)
			{
				_player = GetNode<Player>("../Player.tscn");
			}
			return _player;
		}
	}
	
	//movement Velocity and Frame by Frame moven=ment timer
	private Godot.Vector2 Velocity;
	
	//called on enter
	public override void _Ready()
	{
		fireballSprite = GetNode<AnimatedSprite2D>("FireballSprite");
		fireballSprite.Play("moving");

		//connect signalfor when collides
		Connect("body_entered", new Callable (this, nameof(OnBodyEntered)));
	}

	//update sprite so top is facing direction of travel
	private void UpdateSpriteRotation() {
		if (Velocity.LengthSquared() > 0) {
			fireballSprite.Rotation = Velocity.Angle() + Mathf.Pi / 2;
		}
	}
	
	//sets target, spped, damage
	public void Init(Godot.Vector2 position, Godot.Vector2 target, float speed, int damage)
	{
		Position = position;
		Velocity = (target - position).Normalized() * speed;
		Speed = speed;
		Damage = damage;
	}

	private double alive;
	public async override void _Process(double delta)
	{
		Position += Velocity * (float)delta;

		UpdateSpriteRotation();

		alive += delta;
		if (alive > Lifetime) {
			fireballSprite.Play("burst");
			await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
			QueueFree();
		} 
	}

	//detects collision and does damage
	public async void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			fireballSprite.Play("burst");
			player.SetFireDuration(DebuffDuration);
			await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
			QueueFree();
		}
	}
}
