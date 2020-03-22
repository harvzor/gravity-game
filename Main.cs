using Godot;
using System;

public class Main : Node
{
	private int Score;

	private Hud Hud => base.GetNode<Hud>("Hud");
	private Player Player => base.GetNode<Player>("Player");
	private Goal Goal => base.GetNode<Goal>("Goal");
	private Area2D PlayableArea => base.GetNode<Area2D>("PlayableArea");

	public override void _Ready()
	{
		this.Goal.SetPlayableArea(playableArea: this.PlayableArea.GetViewportRect());
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
