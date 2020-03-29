using Godot;
using System;

public class Goal : Area2D
{
	[Signal]
	public delegate void GoalScored();

	[Export]
	public PackedScene NextScene;

	[Export]
	public Boolean MoveOnGoal;

	private Rect2 PlayableArea;

	private Node2D Spite => base.GetNode<Node2D>("Sprite");

	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public override void _Process(float delta)
	{
		base.RotationDegrees = base.RotationDegrees + 180 * delta;
	}

	public void SetPlayableArea(Vector2[] polygon, Vector2 scale, int margin = 100)
	{
		// Note: maybe I could have just used Vector2.Project ?

		var topLeft = polygon[0];
		var bottomRight = polygon[2];

		var center = new Vector2(
			x: (bottomRight.x - topLeft.x) / 2,
			y: (bottomRight.y - topLeft.y) / 2
		);

		var scaledTopLeft = (topLeft - center * scale) + center;
		var scaledBottomRight = (bottomRight + center * scale) - center;

		var playableArea = new Rect2(
			position: new Vector2(
				x: scaledTopLeft.x + margin,
				y: scaledTopLeft.y + margin
			),
			width: scaledBottomRight.x - scaledTopLeft.x - margin * 2,
			height: scaledBottomRight.y - scaledTopLeft.y - margin * 2
		);

		// playableArea = playableArea;

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
	private void MoveRandom()
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

		if (this.MoveOnGoal)
			this.MoveRandom();
	}
}
