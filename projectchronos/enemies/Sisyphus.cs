using System;
using Godot;

namespace ProjectChronos;

public partial class Sisyphus: BasicEnemy, BasicEnemy.EnemyAI {
	public override void _Ready()
	{
		MaxLife = 1000;
		EnemyHp = MaxLife;
		var healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Value = EnemyHp;
		healthBar.MaxValue = EnemyHp;
		base._Ready();
		sisyphusSprite = GetNode<AnimatedSprite2D>("SisyphusSprite");

	}

	private double sinceLastJump;
	
	private double sinceLastRockRoll;
	private int numberRocksRolled;
	
	private int jumpIdx;

	private double jumpT;

	//instantiate animatedSprite2D node for player sprite
	public AnimatedSprite2D sisyphusSprite;

	public async void ExecuteAI(float delta)
	{
		var velocity = Velocity;

		var jumpEvery = GetMeta("jumpEvery", Variant.From(7d)).AsDouble();

		//flip sprite to face player
		sisyphusSprite.FlipH = PlayerPosition().X < Position.X;
		
		if (sinceLastJump > jumpEvery)
		{
			sinceLastJump -= jumpEvery;
			// Reset rock rolls
			numberRocksRolled = 0;
			sinceLastRockRoll = 0;
			
			if (HasMeta("jumpPos")) 
			{
				var jumpPos = GetMeta("jumpPos").AsFloat64Array();

				if (jumpPos.Length > 0)
				{
					//play jump
					sisyphusSprite.Play("jumping");
					//wait one second
					await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

					
					jumpIdx = (jumpIdx + 1) % jumpPos.Length;
					var nextJump = jumpPos[jumpIdx];

					var speed = GetMeta("speed", Variant.From(1500d)).AsDouble();

					jumpT = Math.Abs(nextJump - Position.X) / speed;

					velocity.X = (float) speed * Math.Sign(nextJump - Position.X);
					velocity.Y = (float)(-0.5 * GetGravity().Y * jumpT);
				}
			}
			
		}

		if (jumpT > 0)
		{
			jumpT -= delta;
			if (jumpT < 0)
			{
				//play idle
				sisyphusSprite.Play("idle");
				velocity = new Vector2(0, 0);
			}
		}
		else
		{
			var rollRockDelay = GetMeta("rollRockDelay", Variant.From(1d)).AsDouble();
			var numberRocksToRoll = GetMeta("numberRocksToRoll", Variant.From(1)).AsInt32();
			
			if (sinceLastRockRoll > rollRockDelay && numberRocksRolled < numberRocksToRoll)
			{
				//wait one second
				//await ToSignal(GetTree().CreateTimer(1.0f), "timeout");

				var rollFrom = new Vector2(Position.X, Position.Y);
				rollFrom.X += Math.Sign((PlayerPosition() - Position).X) *
							  GetNode<CollisionShape2D>("./CollisionShape2D").Shape.GetRect().Size.X * 0.5f;
				
				GetParent().AddChild(Rollingrock.CreateRock(
					rollFrom, 
					PlayerPosition(), 
					(float) GetMeta("rockSpeed", Variant.From(600d)).AsDouble(), 
					GetMeta("rockDamage", Variant.From(5)).AsInt32(), 
					(float) GetMeta("rockKnockbackPercent", Variant.From(0.1d)).AsDouble()
					));
				sinceLastRockRoll -= rollRockDelay;
				numberRocksRolled++;
			} else if (sinceLastRockRoll > rollRockDelay - 0.5 && numberRocksRolled < numberRocksToRoll) {

				//play rolling
				sisyphusSprite.Play("throwing");
			}
			sinceLastRockRoll += delta;
		}
		
		sinceLastJump += delta;

		velocity += GetGravity() * delta;
		Velocity = velocity;
	}

	new Vector2 GetGravity()
	{
		return base.GetGravity();
	}

	public override void kill() {
		GetParent().GetChild<SisyphusExitv2>(9).DefeatedSisyphus();
		QueueFree();
	}
}
