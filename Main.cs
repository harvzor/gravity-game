using Godot;
using System;

public class Main : Node
{
	private int Score;

	private Hud Hud => base.GetNode<Hud>("Hud");
	private Player Player => base.GetNode<Player>("Player");
	private Goal Goal => base.GetNode<Goal>("Goal");

	public override void _Ready()
	{
	}

	public void NewGame()
	{
		this.Score = 0;

		this.Hud.UpdateScore(this.Score);

		this.Player.Show();
	}

	public void GameOver()
	{
		this.Hud.ShowGameOver();
	}

	public void ScoreGoal()
	{
		this.Score++;

		this.Hud.UpdateScore(this.Score);

		this.Goal.MoveRandom();
	}
}
