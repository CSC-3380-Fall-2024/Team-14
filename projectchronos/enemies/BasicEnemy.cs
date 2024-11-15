using Godot;

public partial class BasicEnemy : CharacterBody2D {

	public interface EnemyAI
	{
		void ExecuteAI(float delta);
	}

	public float MaxLife = 100;
	public float CurrentLife = 30;
	
	public float Speed = 300.0f;
	public float JumpVelocity = -400.0f;

	protected EnemyAI ai = new DefaultAI();

	public override void _PhysicsProcess(double delta) {
		TakeDamage( 1 / DistanceToPlayer() * 2000f * (float) delta); // prototype enemy takes passive proximity damage for testing
		
		 // die if we have zero health duh
		if (CurrentLife <= 0) {
			kill();
		}

		ai.ExecuteAI((float) delta); // have to actually run the AI code

		Show();
	}

	// we wrap this in its own method just because? maybe we'll add additional behavior to the kill event
	public void kill() {
		QueueFree();
	}

	public void TakeDamage(float damage) { // should not ever modify enemy health directly (from outside)
		CurrentLife -= damage;
	}

	public Vector2 PlayerPosition() {
		return GetNode<Player>("/root/TartarusLevel/Player").Position;
	}

	public float DistanceToPlayer() {
		return PlayerPosition().DistanceTo(Position); // built-in godot methods sure are useful
	}

	public float GetSpeed() {
		return Speed;
	}
}
