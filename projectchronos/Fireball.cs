using Godot;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class Fireball : Area2D
{
	public float Speed = 200f; //speed in pix/s
	public int Damage = 7; //damage amt

	public static int DebuffDuration = 5; //how long it inflicts debuffs (on fire here)
	public static int Lifetime = 4; // seconds to despawn

	private Player _player;

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

		//connect signalfor when collides
		Connect("body_entered", new Callable (this, nameof(OnBodyEntered)));
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
	public override void _Process(double delta)
	{
		Position += Velocity * (float)delta;

		alive += delta;
		if (alive > Lifetime) QueueFree();
	}

	//detects collision and does damage
	public void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.SetFireDuration(DebuffDuration);
			QueueFree();
		}
	}
}
