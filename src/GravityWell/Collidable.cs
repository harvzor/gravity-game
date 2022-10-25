using Godot;
using System;

public class Collidable : Node2D
{
	[Signal]
	public delegate void Crash();

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

    public override void _Ready()
    {
		this.Connect("body_entered", this, "OnBodyEntered");
    }

	public void Start()
	{
		this.Show();
	}

	public void Stop()
	{
		this.Hide();
	}

	public void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")
			this.EmitSignal("Crash");
	}
}
