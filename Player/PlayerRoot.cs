using Godot;
using System;

public class PlayerRoot : Node2D
{
	[Signal]
	public delegate void Goal();

	[Signal]
	public delegate void GameOver();

    public override void _Ready()
    {

    }
}
