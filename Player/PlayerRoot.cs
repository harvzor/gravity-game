using Godot;
using System;

public class PlayerRoot : Node2D
{
	private Player Player => base.GetNode<Player>("Player");

    public override void _Ready()
    {
		this.Player.SetNewPosition(base.Position);
    }
}
