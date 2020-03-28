using Godot;
using System;

public class GravityWell : Area2D
{
	[Signal]
	public delegate void GameOver();

	private Sun Sun => base.GetNode<Sun>("Sun");

    public override void _Ready()
    {

    }

	public void Start()
	{
		this.Show();

		this.Sun.Start();
	}

	public void Stop()
	{
		this.Hide();

		this.Sun.Stop();
	}

	public void OnGameOver()
	{
		this.EmitSignal("GameOver");
	}
}
