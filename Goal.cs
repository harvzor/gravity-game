using Godot;
using System;

public class Goal : Area2D
{
	private Vector2 ScreenSize;

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public override void _Ready()
	{
		this.ScreenSize = base.GetViewport().Size;

		base.Hide();
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start(Vector2? pos = null)
	{
		if (pos != null)
			base.Position = pos.Value;

		base.Show();
		this.CollisionShape.Disabled = false;
	}

	///<summary>Change the location of the Goal to somewhere random.async</summary>
	public void MoveRandom()
	{
		Random rnd = new Random();

		var randomX = rnd.Next(0, (Int32)this.ScreenSize.x);
		var randomY = rnd.Next(0, (Int32)this.ScreenSize.y);

		// Probably not necessary to camp as the coordinates must already be in the screen.
		base.Position = new Vector2(
			x: Mathf.Clamp(randomX, 0, this.ScreenSize.x),
			y: Mathf.Clamp(randomY, 0, this.ScreenSize.y)
		);
	}
}
