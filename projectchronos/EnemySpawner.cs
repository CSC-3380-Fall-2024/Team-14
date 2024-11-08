using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene {get; set;}

	private void OnSpawnTimerTimeout() {
		Daemon enemy = EnemyScene.Instantiate<Daemon>();
		AddChild(enemy);
	}
}
