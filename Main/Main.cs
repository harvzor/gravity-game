using Godot;
using System;

public class Main : Node
{
	private int Score;

	private Hud Hud => base.GetNode<Hud>("Hud");
	private Player Player => base.GetNode<Player>("Player/Player");
	private Goal Goal => base.GetNode<Goal>("Goal");
	private CollisionPolygon2D PlayableArea => base
		.GetNode<Area2D>("PlayableArea")
		.GetNode<CollisionPolygon2D>("CollisionPolygon2D");

	public override void _Ready()
	{
		var polygon = this.PlayableArea.Polygon;

		var topLeft = polygon[0];
		var bottomRight = polygon[2];

		var playableArea = new Rect2(
			position: this.PlayableArea.Position,
			width: bottomRight.x - topLeft.x,
			height: bottomRight.y - topLeft.y
		);

		this.Goal.SetPlayableArea(playableArea: playableArea);

		this.Player.Stop();
		this.Goal.Stop();
	}

	public void NewGame()
	{
		this.Score = 0;

		this.Hud.UpdateScore(this.Score);

		this.Player.Start();
		this.Goal.Start();
	}

	public void EndGame()
	{
		this.Hud.ShowGameOver();

		this.Player.Stop();
		this.Goal.Stop();
	}

	public void ScoreGoal()
	{
		this.Score++;

		this.Hud.UpdateScore(this.Score);

		this.Goal.MoveRandom();
	}
}
