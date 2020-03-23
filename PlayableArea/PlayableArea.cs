using Godot;
using System;

public class PlayableArea : Area2D
{
	[Signal]
	public delegate void GameOver();

    public override void _Ready()
    {
		// base.Connect("body_shape_exited", this, "OnPlayableAreaBodyExited");
    }

	public void OnPlayableAreaBodyExited(RigidBody2D body)
	{
		if (body.Name == "Player")
			base.EmitSignal("GameOver");
	}
}
