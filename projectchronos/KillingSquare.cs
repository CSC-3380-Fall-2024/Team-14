using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class KillingSquare : Area2D
{
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered))); //connects body entered signal to the method for OnBodyEntered and this signal is only activated when a player enters the big pink square
	}

		private void OnBodyEntered(Node body)
		{
		   if (body is Player player)
		   	{
				GD.Print("collide"); // prints whether or not collision registers **TEST CODE
				player.Kill_Reset(); //kills player
		   	} 
		}
	}
