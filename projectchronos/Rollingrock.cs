using Godot;
using System;

public partial class Rollingrock : CharacterBody2D {

	public Vector2? moveVelocity;
	public Vector2? startingPos;
	public float Speed = 200;
	public Vector2? Target;

	public float DespawnDistance = 2000;
	
	private static double NormalizeAngle(double angle) {
		return (angle % (Math.PI * 2) + Math.PI * 2) % (Math.PI * 2);
	}
	
	public override void _PhysicsProcess(double delta) {
		if (moveVelocity.HasValue && startingPos.HasValue) {

			Vector2 velocity = Velocity;

			// Add the gravity.
			if (IsOnFloor()) {
				velocity = moveVelocity.Value * Speed;
			}
			else {
				velocity += GetGravity() * (float)delta;
			}

			// Delete self when travelled enough distance
			if (startingPos.Value.DistanceTo(Position) > DespawnDistance) {
				QueueFree();
			}
			
			Velocity = velocity;
			MoveAndSlide();
		} else if (Target.HasValue) {
			var angleToMove = NormalizeAngle((Position - Target.Value).Angle() - GetGravity().Angle() + Math.PI / 2);
			var rotateDir = (Math.PI / 2 <= angleToMove && angleToMove <= 3 * Math.PI / 2) ? -1f : 1f;
			moveVelocity = GetGravity().Rotated((float)Math.PI / 2 * rotateDir).Normalized();
			startingPos = Position;
		}
	}

	public static Rollingrock CreateRock(Vector2 position, Vector2 target) {
		var rock = GD.Load<PackedScene>("res://rollingrock.tscn").Instantiate<Rollingrock>();
		rock.Position = position;
		rock.Target = target;
		return rock;
	}
}
