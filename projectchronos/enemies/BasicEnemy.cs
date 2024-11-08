using Godot;

public partial class BasicEnemy : CharacterBody2D {
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public StatBlock stats = new StatBlock(100, 1, true, true, npcAI.MonteKillO, 30);
	public AI ai;

	public override void _Ready()
	{
		ai = MakeAI(stats.ai);
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		TakeDamage( 1 / DistanceToPlayer() * 2000f * (float) delta); // prototype enemy takes passive proximity damage for testing
		
		 // die if we have zero health duh
		if (stats.currentLife <= 0) {
			kill();
		}

		ai.ExecuteAI((float) delta); // have to actually run the AI code

		Show();
	}

	public AI MakeAI(npcAI aiType) {
		switch(aiType) {
			case npcAI.MonteKillO : return new MonteKillO(this, 2);
			default : return new DefaultAI(this);
		}
	}

	// we wrap this in its own method just because? maybe we'll add additional behavior to the kill event
	public void kill() {
		QueueFree();
	}

	public void TakeDamage(float damage) { // should not ever modify enemy health directly (from outside)
		stats.currentLife -= damage;
	}

	public Vector2 PlayerPosition() {
		return GetNode<Player>("/root/Main/World/Player").Position;
	}

	public float DistanceToPlayer() {
		return PlayerPosition().DistanceTo(Position); // built-in godot methods sure are useful
	}

	public float GetSpeed() {
		return Speed;
	}
}


// this is kind of horrible lol this could grow out of control rapidly
public class StatBlock {
	public StatBlock(float maxLife, float damage, bool hasGravity, bool hasCollisions, npcAI ai, float currentLife) {
		this.maxLife = maxLife;
		this.damage = damage;
		this.hasGravity = hasGravity;
		this.hasCollisions = hasCollisions;
		this.ai = ai;
		this.currentLife = currentLife;
	}

	public float maxLife;
	public float damage;
	public bool hasGravity;
	public bool hasCollisions;
	public npcAI ai;
	public float currentLife;

	public override string ToString() => $"({maxLife}, {damage}, {hasGravity}, {hasCollisions}, {ai}, {currentLife})";
}
