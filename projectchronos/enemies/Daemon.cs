using Godot;
using System;

public partial class Daemon : BasicEnemy
{
	public float MeleeRange = 80f; //melee attack range
    
    public float MagicRange = 300f; //magic attack range
	public float EnemyMaxHp = 10f; //enemy health
    

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		TakeDamage( 1 / DistanceToPlayer() * 1000f * (float) delta); 

		GD.Print($"distance to p {DistanceToPlayer()}");
		
			if (DistanceToPlayer() <= MeleeRange) //checks to see if enemy is within attack range
			{
				GD.Print("in range");
				Attack();
			}
			else if(DistanceToPlayer() <= MagicRange)
			{
				GD.Print("in magic range");
				Attack();
			}
            else
            {
                Chase();
            }
		

		if (stats.currentLife <= 0){
			GD.Print("Killing");
			kill();
		}

		Show();
	}

	private void Chase()
	{
		var direction = (PlayerPosition()-Position).Normalized(); //moves towards player
		Velocity = direction * Speed; //sets velocity
		MoveAndSlide();// adds previously estabvlished physics stuff
		GD.Print("chasing");
	}	


	private void Attack()
	{
		GD.Print("Attacking");
		// take damage goes here
		
	}

    private void MagicAttack()
    {
        GD.Print("Fire");
    }

}
