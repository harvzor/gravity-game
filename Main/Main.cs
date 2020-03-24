using Godot;
using System;

public class Main : Node
{
	private int Score;

	private Hud Hud => base.GetNode<Hud>("Hud");
	private Player Player => base.GetNode<Player>("Player");
	private Goal Goal => base.GetNode<Goal>("Goal");
	private CollisionPolygon2D PlayableArea => base
		.GetNode<Area2D>("PlayableArea")
		.GetNode<CollisionPolygon2D>("CollisionPolygon2D");

	private GravityWell GravityWell => base.GetNode<GravityWell>("GravityWell");

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
		this.GravityWell.Stop();
	}

	private void Start()
	{
		this.Player.Start();
		this.Goal.Start();
		this.GravityWell.Start();
	}

	private void Stop()
	{
		this.Player.Stop();
		this.Goal.Stop();
		this.GravityWell.Stop();
	}

	public void NewGame()
	{
		this.UpdateScore(0);

		this.Start();
	}

	private void UpdateScore(int newScore)
	{
		this.Score = newScore;

		this.Hud.UpdateScore(this.Score);
	}

	public void QuitGame()
	{
		this.Stop();
	}

	public void EndGame()
	{
		this.Hud.ShowGameOver();

		this.UpdateScore(0);

		this.Stop();
	}

	public void ScoreGoal()
	{
		this.UpdateScore(this.Score + 1);

		this.Goal.MoveRandom();
	}
}
