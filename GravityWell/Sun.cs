using Godot;
using System;

public class Sun : Area2D
{
	[Signal]
	public delegate void GameOver();

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public void Start()
	{
		// this.CollisionShape.SetDeferred("disabled", false);
	}

	public void Stop()
	{
		// this.CollisionShape.SetDeferred("disabled", true);
	}

	public void OnSunBodyEntered(KinematicBody2D body)
	{
		if (body.Name == "Player")
			this.EmitSignal("GameOver");
	}
}
