using Godot;
using System;

public class Goal : Area2D
{
	private Rect2 PlayableArea;

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public override void _Ready()
	{
		base.Hide();
	}

	public void SetPlayableArea(Rect2 playableArea)
	{
		this.PlayableArea = playableArea;
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		this.MoveRandom();

		base.Show();

		this.CollisionShape.Disabled = false;
	}

	public void Stop()
	{
		base.Hide();

		this.CollisionShape.Disabled = true;
	}

	///<summary>Change the location of the Goal to somewhere random.</summary>
	public void MoveRandom()
	{
		Random rnd = new Random();

		var randomX = rnd.Next((int)this.PlayableArea.Position.x, (int)this.PlayableArea.End.x);
		var randomY = rnd.Next((int)this.PlayableArea.Position.y, (int)this.PlayableArea.End.y);

		base.Position = new Vector2(
			x: randomX,
			y: randomY
		);
	}
}
