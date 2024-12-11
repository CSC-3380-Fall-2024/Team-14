using Godot;
using System;

public partial class Daemon : BasicEnemy, BasicEnemy.EnemyAI
{
	public float MeleeRange = 200f; //melee attack range
	
	public float MagicRange = 600f; //magic attack range

	private float CooldownUntilAttack = 0f; //time until next attack
	private float CooldownTime = 2f; //cooldown in second

	new private float Speed = 300;

	private Player player;

	private PlayerAttack playerAttack;

	PackedScene fireballScene = (PackedScene)ResourceLoader.Load("res://Fireball.tscn");


	//instantiate animatedSprite2D node for player sprite
	public AnimatedSprite2D daemonSprite;

	//flag to check attack state
	private bool isAttacking = false;

	public override void _Ready()
	{
		MaxLife = 40;
		EnemyHp = MaxLife;
		var healthBar = GetNode<HealthBar>("HealthBar");
		healthBar.Value = EnemyHp;
		healthBar.MaxValue = EnemyHp;
	
		base._Ready();
		player = GetParent().GetChild<Player>(5);
		daemonSprite = GetNode<AnimatedSprite2D>("DaemonSprite");

		daemonSprite.Play("flying");

		playerAttack = player.GetChild<PlayerAttack>(6);
	}

	public override void _PhysicsProcess(double delta) {
		if (EnemyHp <= 0) {
			kill();
		}

		DetectHit(); // necessary to take damage

		//flip to face player
		FlipSpriteToPlayer(); 

		if(!isAttacking) {
			ExecuteAI((float)delta);
		}
		MoveAndSlide(); //moves
	}

	private void Chase()
	{
		var direction = (PlayerPosition()-Position).Normalized(); 
		Velocity = direction * Speed; 
		MoveAndSlide();

		//play flying animation
		// daemonSprite.Play("flying");

		//GD.Print("chasing");
	}	


	private void MeleeAttack()
	{
		//play melee attack animation
		daemonSprite.Play("melee");

		isAttacking = true;
		
		var damage = 2;
		player.PlayerHp -= damage;
		CooldownUntilAttack = CooldownTime;
		GD.Print("melee");
		//placeholder for damage

		GetTree().CreateTimer(1f).Connect("timeout", new Callable(this, nameof(OnAttackFinish)));

	}

	private void FireAttack() {
		Velocity = Vector2.Zero;

		daemonSprite.Play("ranged");
		CooldownUntilAttack = CooldownTime;

		GetTree().CreateTimer(0.8f).Connect("timeout", new Callable(this, nameof(SpawnProjectile)));

		//GD.Print("ranged");
	}

	private void OnAttackFinish() {
		isAttacking = false;
    	//resume movement after ranged attack animation finishes
    	daemonSprite.Play("flying");
    	Chase(); 
	}

	 private void SpawnProjectile(){
        // Instantiate fireball scene
        Fireball fireball = (Fireball)fireballScene.Instantiate();
        fireball.Init(Position, player.Position, fireball.Speed, fireball.Damage); // Set target to player position

        GetParent().AddChild(fireball);

        OnAttackFinish(); // Finish the attack
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

		float distanceToPlayer = DistanceToPlayer();

		if (distanceToPlayer <= MeleeRange) {
			if(CooldownUntilAttack <= 0) {
				MeleeAttack();
			} else {
				Chase();
			}
		} else if(distanceToPlayer <= MagicRange) {
			// GD.Print("in fire range");
			if(CooldownUntilAttack <= 0) {
				FireAttack();
			} else {
				Chase();
			}
		} else {
			//GD.Print("Outside range, chasing player");
			Chase();
		}
	}
}
