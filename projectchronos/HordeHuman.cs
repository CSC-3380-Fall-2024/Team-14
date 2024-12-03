using Godot;
using System;

public partial class HordeHuman : BasicEnemy, BasicEnemy.EnemyAI
{
	public float range = 200f; //distance that enemy can attack from
	public float retreat_when_health = 10f; //health that triggers a retreat
	public float retreat_how_far = 1000f; //retreat distance
	public float Speed = 300f;

	private Player player;

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 2f; //cooldown in second

	public float gravity = 500f; // gravity amt
	private Vector2 velocity = Vector2.Zero; //defines velocity

	public float CurrentLife = 35f;


	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("../Player"); //find player

		if (player == null) {
			GD.Print("not found");
			return;
		} 
		else {
			//GD.Print("player found");
			//GD.Print(player.GetPath());
		} // verifies player exists for player hp functionality later

	}
	public override void _PhysicsProcess(double delta) {

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

	private Vector2 PlayerPosition()
	{
		return player.GlobalPosition; //get player pos
	}
	private float DistanceToPlayer()
	{
		return Position.DistanceTo(player.GlobalPosition); //get player distance
	}

	private void chase()
	{		
		var direction = (PlayerPosition()-Position).Normalized(); //moves towards player
		velocity.X = direction.X * Speed; //sets velocity

		//GD.Print("chasing" + velocity);
	}	
	private void run()
	{ 
		//GD.Print("doing run");
		if (DistanceToPlayer() <= retreat_how_far) 
		{
			var direction = (Position - PlayerPosition()).Normalized(); //moves away from player
			velocity.X = direction.X*Speed; //velocity
			//GD.Print("running" + velocity);
		}
	}

	private void attack()
	{

		//GD.Print("Attacking"); TEST**
		// take damage goes here
		player.PlayerHp -= 1;
		CooldownUntilAttack = CooldownTime; //reset cooldwon
		
	}


	public void ExecuteAI(float delta)
	{
		if(CooldownUntilAttack > 0) //update attack cooldown
		{
			CooldownUntilAttack -= (float)delta;
			//GD.Print("cooldown remaining" + CooldownUntilAttack); TEST**
		}

		GD.Print(CurrentLife);

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

	public void TakeDamage(float damage) { // should not ever modify enemy health directly (from outside)
		CurrentLife -= damage;
	}
}
