using Godot;
using System;

public partial class RangedEnemy : CharacterBody2D
{
	public float Speed = 200f;
	public float CloseToPlayer = 1000f; //max chase detection distance
	float StopDist = 450f; //stop if within thsi distance

	public Vector2 Target;
	private CharacterBody2D player;

	public override void _Ready() {
		player = GetNode<Player>("../Player"); //finding player node
		if (player == null)
			{
				GD.Print("Player Not Found");
			}
	}

	public override void _PhysicsProcess(double delta)
	{
		float PlayersDistance = player.Position.DistanceTo(Position); //finds player distance form objcet
		//GD.Print("pd " + PlayersDistance); ** tests how far player is from objcet

		if(PlayersDistance <= CloseToPlayer && PlayersDistance > StopDist) // cxhecks to see if player is in range
		{
			
			//GD.Print("enemy at" + Position); **test
			//GD.Print("p at" + player.Position); *8test
			
			Vector2 Direction = (player.Position - Position).Normalized(); //makes direction of chase
			//GD.Print("Close" + Direction); prints sirevction if in range **test only

			Velocity = Direction * Speed; //set vel

			MoveAndSlide(); //prevphysics for touchung stuff
		}
		else {
			Velocity= Vector2.Zero;//stops movement
			//GD.Print("Not Close to player"); **test statement
		}
	}
}
