using Godot;
using System;

public class Asteroid : Collidable
{
	private Area2D Hitbox => base.GetNode<Area2D>("Hitbox");

    public override void _Ready()
    {
        base._Ready();

		this.Hitbox.Connect("body_entered", this, "OnBodyEntered");
    }
}
