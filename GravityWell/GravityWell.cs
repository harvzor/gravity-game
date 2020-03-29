using Godot;
using System;

public class GravityWell : Area2D
{
	public Sun Sun => base.GetNode<Sun>("Sun");

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
}
