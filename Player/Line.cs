using Godot;
using System;

public class Line : Node2D
{
	public Vector2? DragStartPosition;
	public Vector2? DragCurrentPosition;
	public Vector2? DragEndPosition;

    public override void _Ready()
    {

    }

	public override void _Process(float delta)
	{
		if (this.CanDraw())
			base.Update();
	}

	private bool CanDraw()
	{
		return this.DragStartPosition != null && (this.DragCurrentPosition != null || this.DragEndPosition != null);
	}

	public override void _Draw()
	{
		if (this.CanDraw() == false)
			return;

		this.DrawLine(
			from: this.DragStartPosition.Value,
			to: this.DragEndPosition != null
				? this.DragEndPosition.Value
				: this.DragCurrentPosition.Value,
			color: Color.ColorN("red"),
			width: 3
		);
	}
}

