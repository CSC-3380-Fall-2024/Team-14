using Godot;

public partial class MeleeEnemy : BasicEnemy, BasicEnemy.EnemyAI {
	public float range = 200f; //distance that enemy can attack from
	public float retreat_when_health = 10f; //health that triggers a retreat
	public float retreat_how_far = 1000f; //retreat distance

	private Player player;

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 2f; //cooldown in second

	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("../Player"); //find player

		ai = this;
		if (player == null) {
			GD.Print("not found");
			return;
		} 
		else {
			GD.Print("player found");
			GD.Print(player.GetPath());
		}// verifies player exists for player hp functionality later

	}
	
	private Vector2 PlayerPosition()
	{
		return player.GlobalPosition;
	}
	private float DistanceToPlayer()
	{
		return Position.DistanceTo(player.GlobalPosition);
	}

	private void chase()
	{
		
		var direction = (PlayerPosition()-Position).Normalized(); //moves towards player
		Velocity = direction * Speed; //sets velocity
		MoveAndSlide();// adds previously estabvlished physics stuff
	}	
	private void run()
	{ 
		//GD.Print("doing run");
		if (DistanceToPlayer() <= retreat_how_far) 
		{
			var direction = (Position - PlayerPosition()).Normalized(); //moves away from player
			Velocity = direction*Speed; //velocity
			MoveAndSlide(); //prev physics
			//GD.Print("leaving");
		}
	}

	private void attack()
	{

		//GD.Print("Attacking"); TEST**
		// take damage goes here
		player.PlayerHp -= 2;
		CooldownUntilAttack = CooldownTime; //reset cooldwon
		
	}


	public void ExecuteAI(float delta)
	{
		if(CooldownUntilAttack > 0) //update attack cooldown
		{
			CooldownUntilAttack -= (float)delta;
			//GD.Print("cooldown remaining" + CooldownUntilAttack); TEST**
		}

		//GD.Print("distance to p" + distanceToPlayer); **TEST
		if (CurrentLife > retreat_when_health) // checks to see if the enemies health is above the retreat value
		{
			if (DistanceToPlayer() <= range) //checks to see if enemy is within attack range
			{
				//GD.Print("in range"); **TEST
				if (CooldownUntilAttack <= 0) //if colldown end attack player again
				{
					attack();
				}
			}
			else
			{
				chase();
			}
		}
		else
		{
			run();
		}
	}
}
