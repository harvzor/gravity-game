using Godot;
using System;

public class PlayableArea : Area2D
{
	public static Boolean IgnoreCollisionsOnce = false;

	[Signal]
	public delegate void GameOver();

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
			base.EmitSignal("GameOver");
	}
}
