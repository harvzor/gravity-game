using Godot;
using System;

public class Sun : Area2D
{
	[Signal]
	public delegate void GameOver();

    public override void _Ready()
    {

    }

	public void OnSunBodyEntered(KinematicBody2D body)
	{
		if (body.Name == "Player")
			this.EmitSignal("GameOver");
	}
}
