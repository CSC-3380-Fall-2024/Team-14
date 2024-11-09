using Godot;
using System;

public partial class Daemon : BasicEnemy
{
	public float MeleeRange = 150f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range
	public float EnemyMaxHp = 10f; //enemy health
	

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		TakeDamage( 1 / DistanceToPlayer() * 1000f * (float) delta); 

		//GD.Print($"distance to p {DistanceToPlayer()}");
		
			if (DistanceToPlayer() <= MeleeRange) //checks to see if enemy is within attack range
			{
				//GD.Print("in melee range");
				MeleeAttack();
			}
			else if(DistanceToPlayer() <= MagicRange)
			{
				//GD.Print("in magic range");
				FireAttack();
			}
			else
			{
				Chase();
			}
		

		if (stats.currentLife <= 0){
			//GD.Print("Killing");
			kill(); //removes enemy when health hits zero
		}

		Show();
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
		var damage = 5;
		var play = GetNode<Player>("Player");
		play.PlayerHp -= damage;
		//placeholder for damage
		
	}

	private void FireAttack()
	{
		//placeholder method for animations
		var damage = 6;
		var play = GetNode<Player>("Player");
		play.PlayerHp -= damage;
		play.SetFireDuration(5);
		//another placeholder
	}

}
