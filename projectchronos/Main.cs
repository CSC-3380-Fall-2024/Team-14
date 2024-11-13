using Godot;
using System;
using System.Numerics;
using ProjectChronos;

public partial class Main : Node {

	private Configuration config = new() { MaxLives = 3, FireDamagePerSecond = 2 };
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		
		//Call start on player
		Player player = GetNode<Player>("World/Player");
		player.Start(new Godot.Vector2 (0,0));
		GetTree().Paused = false;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public Configuration getConfig() {
		return this.config;
	}
}
