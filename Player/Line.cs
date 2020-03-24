using Godot;
using System;

public class Line : Node2D
{
	public bool Dragging;
	public Vector2? DragCurrentPosition;
	public Vector2? DragEndPosition;

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
			to:
				(
					this.DragEndPosition != null
						? this.DragEndPosition.Value
						: this.DragCurrentPosition.Value
				)
				- this.Player.Position,
			color: Color.ColorN("red"),
			width: 3
		);
	}
}

