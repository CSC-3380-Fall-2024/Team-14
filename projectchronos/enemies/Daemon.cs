using Godot;
using System;

public partial class Daemon : BasicEnemy, BasicEnemy.EnemyAI
{
	public float MeleeRange = 200f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range

	private Vector2 velocity = Vector2.Zero;

	public float gravity = 0f;

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 4f; //cooldown in second


	private Player player;

	public override void _Ready()
	{
		CurrentLife = 45f;
		ai = this;
		base._Ready();
		player = GetParent().GetChild<Player>(5);
	}

	public override void _PhysicsProcess(double delta)
	{
		TakeDamage( 1 / DistanceToPlayer() * 2000f * (float) delta); // prototype enemy takes passive proximity damage for testing
		if (CurrentLife <= 0) {
			kill();
		}

		velocity = Velocity; // updates vel
		if (!IsOnFloor()){
			velocity.Y += gravity * (float)delta;
		}
		else {
			velocity.Y = 0;
		} //checks for if it is already on the floor
		ExecuteAI((float)delta);
		Velocity = velocity; //updates vel again
		MoveAndSlide(); //moves
	}
	private void Chase()
	{
		var direction = (PlayerPosition()-Position).Normalized(); 
		Velocity = direction * Speed; 
		MoveAndSlide();
		//GD.Print("chasing");
	}	


	private void MeleeAttack()
	{
		//placeholder method for animations
		var damage = 4;
		player.PlayerHp -= damage;
		CooldownUntilAttack = CooldownTime;
		
	}

	private void FireAttack()
	{
		//placeholder method for animations
		var damage = 3;
		player.PlayerHp -= damage;
		player.playerSprite.Play("Fire Animation");
		CooldownUntilAttack = CooldownTime;
	}

	public void ExecuteAI(float delta)
	{
		if(CooldownUntilAttack > 0) 
		{
			CooldownUntilAttack -= delta;
		}
		if (DistanceToPlayer() <= MeleeRange) //checks to see if enemy is within attack range
		{
			//GD.Print("in melee range");
			if (CooldownUntilAttack <= 0) 
				{
					MeleeAttack();
				}
				else
				{
					Chase();
				}
		}
		else if(DistanceToPlayer() <= MagicRange)
		{
			if (CooldownUntilAttack <= 0) 
				{
					//FireAttack();
				}
				else
				{
					Chase();
				}
	}
}
}
