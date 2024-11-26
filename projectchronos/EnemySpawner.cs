using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	public Timer SpawnTimer;
	[Export]
	public PackedScene EnemyScene {get; set;}
	public override void _Ready()
	{
		base._Ready();
		SpawnTimer = new Timer();
		SpawnTimer.WaitTime = 6;
		SpawnTimer.OneShot = false;
		SpawnTimer.Autostart = true;
		SpawnTimer.Connect("timeout", new Callable(this, "OnSpawnTimerTimeout"));
		AddChild(SpawnTimer);
	}
	private void OnSpawnTimerTimeout() {
		Daemon enemy = EnemyScene.Instantiate<Daemon>();
		GetParent().AddChild(enemy);
	}
}
