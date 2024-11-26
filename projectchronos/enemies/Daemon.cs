using Godot;
using System;

public partial class Daemon : BasicEnemy, BasicEnemy.EnemyAI
{
	public float MeleeRange = 200f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 3f; //cooldown in second

	private Player player;

	public override void _Ready()
	{
		CurrentLife = 45f;
		ai = this;
		base._Ready();
		player = GetParent().GetChild<Player>(5);
	}

	public override void _PhysicsProcess(double delta) {

		TakeDamage( 1 / DistanceToPlayer() * 2000f * (float) delta); // prototype enemy takes passive proximity damage for testing
		if (CurrentLife <= 0) {
			kill();
		}
		ExecuteAI((float)delta);
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
		GD.Print("melee");
		//placeholder for damage
		
	}

	private void FireAttack()
	{
		//placeholder method for animations
		var damage = 3;
		player.PlayerHp -= damage;
		player.SetFireDuration(5);
		CooldownUntilAttack = CooldownTime;
		GD.Print("fire");
		//another placeholder
	}

	public void ExecuteAI(float delta)
	{
		if(CooldownUntilAttack > 0) //update attack cooldown
		{
			CooldownUntilAttack -= (float)delta;
			//GD.Print("cooldown remaining" + CooldownUntilAttack); TEST**
		}

		if (DistanceToPlayer() <= MeleeRange) //checks to see if enemy is within attack range
		{
				if (CooldownUntilAttack <= 0) //if colldown end attack player again
				{
					MeleeAttack();
				}
		}
		else if(DistanceToPlayer() <= MagicRange)
		{
				if (CooldownUntilAttack <= 0) //if colldown end attack player again
				{
					FireAttack();
				}
		}
		else
		{
			Chase();
		}
	}
	public void TakeDamage(float damage) { // should not ever modify enemy health directly (from outside)
		CurrentLife -= damage;
	}
}
