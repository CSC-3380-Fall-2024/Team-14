using Godot;
using System;

public partial class RockDespawn : Area2D
{
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered))); //connects body entered signal to the method for OnBodyEntered and this signal is only activated when a player enters the big pink square
	}

	private void OnBodyEntered(Node body) {
		if (body is Rollingrock rollingrock) {
			rollingrock.QueueFree();
		} 
	}
}
