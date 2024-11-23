using Godot;
using System;

public partial class Daemon : BasicEnemy, BasicEnemy.EnemyAI
{
	public float MeleeRange = 200f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range

	private Player player;

	public override void _Ready()
	{
		MaxLife = 10f;
		CurrentLife = 10f;
		ai = this;
		base._Ready();
		player = GetParent().GetChild<Player>(5);
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
		player.PlayerHp -= damage;
		//placeholder for damage
		
	}

	private void FireAttack()
	{
		//placeholder method for animations
		var damage = 6;
		player.PlayerHp -= damage;
		player.SetFireDuration(5);
		//another placeholder
	}

	public void ExecuteAI(float delta)
	{
		if (DistanceToPlayer() <= MeleeRange) //checks to see if enemy is within attack range
		{
			//GD.Print("in melee range");
			//MeleeAttack();
		}
		else if(DistanceToPlayer() <= MagicRange)
		{
			//GD.Print("in magic range");
			//FireAttack();
		}
		else
		{
			Chase();
		}
	}
}
