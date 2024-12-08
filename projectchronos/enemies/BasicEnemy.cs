using Godot;

public partial class BasicEnemy : CharacterBody2D {

	public interface EnemyAI
	{
		void ExecuteAI(float delta);
	}

	public int MaxLife = 1000;
	
	
	public float Speed = 300.0f;
	public float JumpVelocity = -400.0f;

	protected NodePath healthBarPath ="HealthBar"; 

	private int _enemyHp;

	public int EnemyHp {
		get => _enemyHp;
		set {
			_enemyHp = (int)Mathf.Clamp(value, 0, MaxLife);
			if( HasNode(healthBarPath) && GetNode(healthBarPath) is HealthBar healthBar){
			healthBar.MaxValue = MaxLife;
			healthBar.Value = _enemyHp;
			}
		}
	}

	private Player _player;
	private Player player
	{
		get
		{
			if (_player == null) _player = GetNode<Player>("../Player");
			return _player;
		}
	}

	private PlayerAttack playerAttack;

	public override void _Ready() {
		playerAttack = player.GetChild<PlayerAttack>(6);
		
	}

	public override void _PhysicsProcess(double delta) {
		DetectHit(); // necessary to take damage

		// die if we have zero health duh
		if (EnemyHp <= 0) {
			kill();
		}

		if (this is EnemyAI ai) ai.ExecuteAI((float) delta);

		MoveAndSlide();
		Show();
	}

	public void DetectHit() {
		if (DistanceToPlayer() <= 400f && !playerAttack.GetChild<Timer>(1).IsStopped()) {
			if (playerAttack.GetChild<Timer>(1).TimeLeft < (playerAttack.AttackPeriod() / 20)) {
				TakeDamage(player.GetChild<PlayerAttack>(6).ScaledDamage());
			}
		}
	}

	// we wrap this in its own method just because? maybe we'll add additional behavior to the kill event
	public virtual void kill() {
		QueueFree();
	}

	public void TakeDamage(int damage) { // should not ever modify enemy health directly (from outside)
		EnemyHp -= damage;
	}

	public Vector2 PlayerPosition()
	{
		return player.Position;
	}

	public float DistanceToPlayer() {
		return PlayerPosition().DistanceTo(Position); // built-in godot methods sure are useful
	}

	public float GetSpeed() {
		return Speed;
	}
}
