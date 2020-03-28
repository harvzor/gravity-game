using Godot;
using System;

public class PlayableArea : Area2D
{
	[Signal]
	public delegate void GameOver();

	public CollisionPolygon2D CollisionShape => base.GetNode<CollisionPolygon2D>("CollisionShape");

    public override void _Ready()
    {
		// base.Connect("body_shape_exited", this, "OnPlayableAreaBodyExited");
    }

	public void OnPlayableAreaBodyExited(RigidBody2D body)
	{
		if (this.CollisionShape.Disabled == true)
			return;

		if (body.Name == "Player")
			base.EmitSignal("GameOver");
	}
}
