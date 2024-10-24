using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class BasicEnemy : CharacterBody2D {
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private StatBlock stats = new StatBlock(100, 1, true, true, npcAI.Default, 100);

    public override void _PhysicsProcess(double delta) {
		TakeDamage(20 * (float) delta); // prototype enemy takes passive damage for testing
		stats = AiProcess(stats); // run the 'AI'
		Show();
	}

	// AiProcess mutates NPC stats based on behavior defined with the given 'AI'
	public StatBlock AiProcess(StatBlock stats) {
		if (stats.ai == npcAI.Default) {
			if (stats.currentLife <= 0) {
				kill();
			}
		}
		return stats;
	}

	// we wrap this in its own method just because? maybe we'll add additional behavior to the kill event
	public void kill() {
		QueueFree();
	}

	public void TakeDamage(float damage) {
		stats.currentLife -= damage;
	}
}

public enum npcAI {
	Default // will be adding more for actual behaviors
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