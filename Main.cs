using Godot;
using System;

public class Main : Node
{
	// [Export]
	// public PackedScene Goal;

	private int Score;

	private Hud GetHud() => base.GetNode<Hud>("Hud");
	private Player GetPlayer() => base.GetNode<Player>("Player");

	public override void _Ready()
	{
	}

	public void NewGame()
	{
		this.Score = 0;

		var hud = this.GetHud();

		hud.UpdateScore(this.Score);

		var player = this.GetPlayer();

		player.Show();
	}

	public void GameOver()
	{
		this.GetHud().ShowGameOver();
	}

	public void ScoreGoal()
	{
		this.Score++;

		var hud = this.GetHud();

		hud.UpdateScore(this.Score);
	}
}
