using Godot;
using System;

public class GravityWell : Area2D
{
	[Signal]
	public delegate void GameOver();

    public override void _Ready()
    {

    }

	public void OnGameOver()
	{
		this.EmitSignal("GameOver");
	}
}
