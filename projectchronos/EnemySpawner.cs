using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene {get; set;}
	public override void _Ready()
	{
		base._Ready();
		var SpawnTimer = new Timer();
		SpawnTimer.WaitTime = 2;
		SpawnTimer.OneShot = false;
		SpawnTimer.Autostart = true;
		SpawnTimer.Connect("timeout", new Callable(this, "OnSpawnTimerTimeout"));
		AddChild(SpawnTimer);
	}
	private void OnSpawnTimerTimeout() {
		Daemon enemy = EnemyScene.Instantiate<Daemon>();
		AddChild(enemy);
	}
}
