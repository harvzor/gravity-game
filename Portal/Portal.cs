using Godot;
using System;

public class Portal : Area2D
{
	[Export]
	public NodePath ConnectingPortalPath;

	private Portal ConnectingPortal => base.GetNode<Portal>(this.ConnectingPortalPath);

    public override void _Ready()
    {
        this.Connect("body_entered", this, "OnBodyEntered");
    }

	public void OnBodyEntered(RigidBody2D body)
	{
		if (body is Player player)
		{
            if (player.CanTeleport)
                player.Position = this.ConnectingPortal.Position;
		}
	}
}
