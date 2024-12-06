using Godot;

public partial class MeleeEnemy : BasicEnemy, BasicEnemy.EnemyAI {
	public float range = 200f; //distance that enemy can attack from
	public float retreat_when_health = 10f; //health that triggers a retreat
	public float retreat_how_far = 1000f; //retreat distance

	private Player player;

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 1.5f; //cooldown in second


	public float gravity = 500f; // gravity amt
	private Vector2 velocity = Vector2.Zero; //defines velocity

	//instantiate animatedSprite2D node for player sprite
	public AnimatedSprite2D meleeEnemySprite;



	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("../Player"); //find player

		if (player == null) {
			GD.Print("not found");
			return;
		} 
		else {
			GD.Print("player found");
			GD.Print(player.GetPath());
		} // verifies player exists for player hp functionality later

		meleeEnemySprite = GetNode<AnimatedSprite2D>("MeleeEnemySprite");
		meleeEnemySprite.Play("idle");

	}
	public override void _PhysicsProcess(double delta) {

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

		DetectHit(); // necessary to take damage
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
		player.PlayerHp -= 5;
		CooldownUntilAttack = CooldownTime; //reset cooldwon
		
	}


	public void ExecuteAI(float delta) {

		if(CooldownUntilAttack > 0) //update attack cooldown
		{
			CooldownUntilAttack -= (float)delta;
			//GD.Print("cooldown remaining" + CooldownUntilAttack); TEST**
		}

		//GD.Print("distance to p" + distanceToPlayer); **TEST
		if (CurrentLife > retreat_when_health) // checks to see if the enemies health is above the retreat value
		{
			//flip sprite to face player based on player position
			meleeEnemySprite.FlipH = PlayerPosition().X < Position.X;

			if (DistanceToPlayer() <= range) //checks to see if enemy is within attack range
			{
				//GD.Print("in range"); **TEST
				if (CooldownUntilAttack <= 0) //if colldown end attack player again
				{
					meleeEnemySprite.Play("melee");
					attack();
				}
			}
			else
			{
				meleeEnemySprite.Play("walking");
				chase();
			}
		}
		else
		{
			//flip sprite to face away from player based on player position
			meleeEnemySprite.FlipH = PlayerPosition().X > Position.X;

			meleeEnemySprite.Play("walking");
			run();
		}
	}
}
