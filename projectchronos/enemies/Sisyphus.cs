using System;
using Godot;

namespace ProjectChronos;

public partial class Sisyphus: BasicEnemy, BasicEnemy.EnemyAI {
	public override void _Ready()
	{
		MaxLife = 1000;
		CurrentLife = 1000;
		base._Ready();
	}

	private double sinceLastJump;
	
	private double sinceLastRockRoll;
	private int numberRocksRolled;
	
	private int jumpIdx;

	private double jumpT;

	public void ExecuteAI(float delta)
	{
		var velocity = Velocity;

		var jumpEvery = GetMeta("jumpEvery", Variant.From(7d)).AsDouble();
		
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
				velocity = new Vector2(0, 0);
			}
		}
		else
		{
			var rollRockDelay = GetMeta("rollRockDelay", Variant.From(1d)).AsDouble();
			var numberRocksToRoll = GetMeta("numberRocksToRoll", Variant.From(1)).AsInt32();
			
			if (sinceLastRockRoll > rollRockDelay && numberRocksRolled < numberRocksToRoll)
			{
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
			}
			sinceLastRockRoll += delta;
		}
		
		sinceLastJump += delta;

		velocity += GetGravity() * delta;
		Velocity = velocity;
		MoveAndSlide();
	}

	new Vector2 GetGravity()
	{
		return base.GetGravity() * 2;
	}
}
