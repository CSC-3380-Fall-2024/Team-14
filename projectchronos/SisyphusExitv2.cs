using Godot;
using System;

public partial class SisyphusExitv2 : Area2D {
	private Player player;
	private bool changeScene = false;
	private bool isSisyphusDefeated = false;

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

		//connect signal for when collides
		Connect("body_entered", new Callable (this, nameof(OnBodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (changeScene) {
			((Main) GetTree().CurrentScene).SwitchLevel("res://the_end.tscn");
		}
	}

	public void DefeatedSisyphus() {
		isSisyphusDefeated = true;
	}

	public void OnBodyEntered(Node body) {
		if (body is Player && isSisyphusDefeated) {
			changeScene = true;
		}
	}
}
