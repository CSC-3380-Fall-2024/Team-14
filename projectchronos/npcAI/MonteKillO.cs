using Godot;
using System;

class MonteKillO : BasicEnemy.EnemyAI {
	private BasicEnemy enemy;
	private Vector2 target;
	private bool hasTarget = false;
	private float aggroRange = 0;
	private Random rnd = new Random();
	private Vector2 moveVec;

	public MonteKillO (BasicEnemy enemy) {
		this.enemy = enemy;
	}

	public MonteKillO(BasicEnemy enemy, float aggroRange) {
		this.enemy = enemy;
		this.aggroRange = aggroRange;
	}

	// this is the principle entry-point of AI behavior
	public void ExecuteAI(float delta) {
		CheckTarget();
		Move(delta); // delta is necessary for AI processing (and not just for movement)
	}

	// just zoooming!
	private void Move(float delta) {
		enemy.Position = enemy.Position.DirectionTo(enemy.PlayerPosition()) * enemy.GetSpeed() * delta + enemy.Position;
	}

	// remember to use this during execution
	private void CheckTarget() {
		hasTarget = !enemy.Position.Equals(enemy.PlayerPosition());
	}

	// how to choose a location to move toward
	private Vector2 GetTarget() {
		if (enemy.DistanceToPlayer() <= aggroRange) { // AI targets player if close enough
			return enemy.PlayerPosition();
		} else {
			if (hasTarget) {
				return target; // keep previous
			} else {
				// choose new random direction
				// distance proportional to distance from player
				return (new Vector2( rnd.Next(), rnd.Next()).Normalized() * enemy.DistanceToPlayer()) + enemy.Position;
			}
		}
	}
}
