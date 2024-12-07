using Godot;
using System;

public partial class Daemon : BasicEnemy, BasicEnemy.EnemyAI
{
	public float MeleeRange = 200f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 2f; //cooldown in second

	new private float Speed = 400;

	private Player player;

	//instantiate animatedSprite2D node for player sprite
	public AnimatedSprite2D daemonSprite;

	public override void _Ready()
	{
		CurrentLife = 30f;
		base._Ready();
		player = GetParent().GetChild<Player>(5);
		daemonSprite = GetNode<AnimatedSprite2D>("DaemonSprite");

		daemonSprite.Play("flying");
	}

	public override void _PhysicsProcess(double delta) {

		TakeDamage( 1 / DistanceToPlayer() * 2000f * (float) delta); // prototype enemy takes passive proximity damage for testing
		if (CurrentLife <= 0) {
			kill();
		}

		//flip to face player
		FlipSpriteToPlayer(); 

		ExecuteAI((float)delta);
		MoveAndSlide(); //moves
	}

	private void Chase()
	{
		var direction = (PlayerPosition()-Position).Normalized(); 
		Velocity = direction * Speed; 
		MoveAndSlide();

		//play flying animation
		daemonSprite.Play("flying");

		//GD.Print("chasing");
	}	


	private void MeleeAttack()
	{
		//play melee attack animation
		daemonSprite.Play("melee");
		
		var damage = 4;
		player.PlayerHp -= damage;
		CooldownUntilAttack = CooldownTime;
		GD.Print("melee");
		//placeholder for damage
		
	}

	private async void FireAttack()
	{
		//play ranged animation
		daemonSprite.Play("ranged");
		
		var damage = 3;
		player.PlayerHp -= damage;
		player.SetFireDuration(5);
		CooldownUntilAttack = CooldownTime;
		GD.Print("fire");
		//another placeholder

		//wait for animation to finish
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		//play flying animation again
		daemonSprite.Play("flying");
	}

	//helper method to make the sprite always face the player
	private void FlipSpriteToPlayer() {
		//flip sprite to face player based on player position
		daemonSprite.FlipH = PlayerPosition().X > Position.X;
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
					//FireAttack();
				}
		}
		else
		{
			Chase();
		}
	}
}
