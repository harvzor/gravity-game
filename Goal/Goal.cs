using Godot;
using System;

public class Goal : Area2D
{
	[Signal]
	public delegate void GoalScored();

	[Export]
	public PackedScene NextScene;

	private Rect2 PlayableArea;

	private Node2D Spite => base.GetNode<Node2D>("Sprite");

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public override void _Process(float delta)
	{
		base.RotationDegrees = base.RotationDegrees + 180 * delta;
	}

	public void SetPlayableArea(Vector2[] polygon)
	{
		var topLeft = polygon[0];
		var bottomRight = polygon[2];

		var playableArea = new Rect2(
			position: this.PlayableArea.Position,
			width: bottomRight.x - topLeft.x,
			height: bottomRight.y - topLeft.y
		);

		this.PlayableArea = playableArea;
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		base.Show();

		// this.CollisionShape.SetDeferred("disabled", false);
	}

	public void Stop()
	{
		base.Hide();

		// this.CollisionShape.SetDeferred("disabled", true);
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

	public void OnGoalBodyEntered(KinematicBody2D body)
	{
		if (body.Name == "Player")
			base.EmitSignal("GoalScored");

		if (this.NextScene != null)
			base.GetTree().ChangeSceneTo(this.NextScene);
	}
}
