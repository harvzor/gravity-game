using Godot;
using System;

public class Line : Node2D
{
	public bool Dragging;
	public Vector2? DragCurrentPosition;

	private Player Player => base.GetNode<Player>("../");

    public override void _Ready()
    {

    }

	public override void _Process(float delta)
	{
		base.Update();
	}

	public override void _Draw()
	{
		if (this.Dragging == false)
		{
			// Remove the line.
			this.DrawLine(new Vector2(0, 0), new Vector2(0, 0), Color.ColorN("white"));

			return;
		}

		this.DrawLine(
			from: new Vector2(x: 0, y: 0),
			// The line to position needs to be counter rotated to the player position
			// because the line is drawn in relation to the player direction.
			to: (this.DragCurrentPosition.Value - this.Player.Position).Rotated(-this.Player.Rotation),
			color: Color.ColorN("red"),
			width: 3 * this.Player.Camera.Zoom.x
		);
	}
}

