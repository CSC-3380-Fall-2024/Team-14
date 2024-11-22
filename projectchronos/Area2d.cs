using Godot;
using System;

public partial class Area2d : Area2D {
	private Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		base._Ready();
		player = GetNode<Player>("../Player"); //find player

		if (player == null) {
			GD.Print("not found");
			return;
		} 
		else {
			GD.Print("player found");
			GD.Print(player.GetPath());
		}

		//connect signalfor when collides
		Connect("body_entered", new Callable (this, nameof(OnBodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public void OnBodyEntered(Node body) {
		if (body is Player) {
			// Chaneg scene
			((Main) GetTree().CurrentScene).SwitchLevel("res://elysium_level.tscn");
		}
	}
}
