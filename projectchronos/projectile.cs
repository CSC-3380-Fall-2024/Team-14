using Godot;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class projectile : Area2D
{
	public float Speed = 200f; //speed in pix/s
	public int Damage = 7; //damage amt
	private CharacterBody2D player; //refrences player
	
	//movement Velocity and Frame by Frame moven=ment timer
	private Godot.Vector2 Velocity = new Godot.Vector2();
	private Timer MoverTimer;
	
	//despawns after 3 sec
	private Timer Despawner;
	
	//called on enter
	public override void _Ready()
	{
		//find player node
		player = GetNode<CharacterBody2D>("../Player");
		//set pos, speed, damage
		SetTarget(player.GlobalPosition, Speed, Damage);

		//connect signalfor when collides
		Connect("body_entered", new Callable (this, nameof(OnBodyEntered)));
	
		//starts mover timer
		MoverTimer = GetNode<Timer>("MoverTimer");
		MoverTimer.Connect("timeout", new Callable (this, nameof(OnMoveTimerTimeout)));
		MoverTimer.Start(0.016f); //60 fps move timer
		
		//starts despawner timer
		Despawner = GetNode<Timer>("Despawner");
		Despawner.Connect("timeout", new Callable (this, nameof(OnDespawnerTimeout)));
		Despawner.Start(3f); //3 second limit
	}
	
	//sets target, spped, damage
	public void SetTarget(Godot.Vector2 target, float Speed, int Damage)
	{
		//how to get from curent posit to player posit
		Godot.Vector2 Direction = (target - GlobalPosition).Normalized();
		if(GlobalPosition.X > target.X) //if player to left
		{
			Direction.X = -Mathf.Abs(Direction.X);// - direction (moves left)
		}
		if(GlobalPosition.X < target.X) //if player to right
		{
			Direction.X = Mathf.Abs(Direction.X); //+ direction (moves right)
		}
		
		//sets velocity
		Velocity = (Direction * Speed);
		
	}
	
	//updateposition of projectile
	public void OnMoveTimerTimeout(){
		float delta = (float)MoverTimer.WaitTime;
		Position += Velocity * delta;
	}
	
	//kills projectile after 3s
	public void OnDespawnerTimeout(){
		QueueFree();
	}
	
	//detects collision and does damage
	public void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.PlayerHp -= Damage;
			//GD.Print("health" + player.PlayerHp); **test code**
			QueueFree();
		}
	}
}
