using Godot;

public partial class MeleeEnemy : BasicEnemy {
	public float range = 60f; //distance that enemy can attack from
	public float retreat_when_health = 20f; //health that triggers a retreat
	public float retreat_how_far = 200f; //retreat distance
	

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		//TakeDamage( 1 / DistanceToPlayer() * 1000f * (float) delta); // borrowed from basic cause it isnt implementing through inheritance??

		GD.Print($"distance to p {DistanceToPlayer()}");
		if (stats.currentLife > retreat_when_health) // checks to see if the enemies health is above the retreat value
		{
			if (DistanceToPlayer() <= range) //checks to see if enemy is within attack range
			{
				GD.Print("in range");
				attack();
			}
			else
			{
				GD.Print("running");
				chase();
			}
		}
		else
		{
			run();
		}

		if (stats.currentLife <= 0){
			GD.Print("Killing");
			kill();
		}

		Show();
	}

	private void chase()
	{
		var direction = (PlayerPosition()-Position).Normalized(); //moves towards player
		Velocity = direction * Speed; //sets velocity
		MoveAndSlide();// adds previously estabvlished physics stuff
		GD.Print("chasing");
	}	
	private void run()
	{ 
		if (DistanceToPlayer() <= retreat_how_far) 
		{
			var direction = (Position - PlayerPosition()).Normalized(); //moves away from player
			Velocity = direction*Speed; //velocity
			MoveAndSlide(); //prev physics
			GD.Print("leaving");
		}
	}

	private void attack()
	{
		GD.Print("Attacking");
		// take damage goes here
		
	}

	
}
