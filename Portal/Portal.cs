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
        this.Connect("body_exited", this, "OnBodyExited");
    }

	public void OnBodyEntered(RigidBody2D body)
	{
		if (body is Player player)
		{
            if (player.CanTeleport == true)
            {
                player.Position = this.ConnectingPortal.Position;
                player.CanTeleport = false;
                player.LinePath.NewLine = true;
                player.LinePath.Draw = false;
            }
		}
	}

	public void OnBodyExited(RigidBody2D body)
	{
		if (body is Player player)
		{
            if (player.CanTeleport == false)
            {
                player.CanTeleport = true;
                player.LinePath.Draw = true;
            }
		}
	}
}
