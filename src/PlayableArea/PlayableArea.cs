using Godot;
using System;
using System.Linq;

public class PlayableArea : Area2D
{
	public static Boolean IgnoreCollisionsOnce = false;

	[Signal]
	public delegate void Crash();

	public CollisionPolygon2D CollisionShape => base.GetNode<CollisionPolygon2D>("CollisionShape");

    public override void _Ready()
    {
		IgnoreCollisionsOnce = false;
    }

	public void OnPlayableAreaBodyExited(RigidBody2D body)
	{
		if (IgnoreCollisionsOnce == true || this.CollisionShape.Disabled == true)
		{
			IgnoreCollisionsOnce = false;

			return;
		}

		if (body.Name == "Player")
			base.EmitSignal("Crash");
	}
}
