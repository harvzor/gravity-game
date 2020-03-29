using Godot;
using System;

public class Collidable : Area2D
{
	[Signal]
	public delegate void Crash();

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public void Start()
	{
		this.Show();
	}

	public void Stop()
	{
		this.Hide();
	}

	public void OnSunBodyEntered(KinematicBody2D body)
	{
		if (body.Name == "Player")
			this.EmitSignal("Crash");
	}
}
