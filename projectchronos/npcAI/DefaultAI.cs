using Godot;
using System;

public class DefaultAI : AI {
    public BasicEnemy enemy;
	public DefaultAI (BasicEnemy enemy) {
        this.enemy = enemy;
	}

	public override void ExecuteAI(float delta) {
    }
}