using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Main : Node
{
	private int Score;

	private Controls Controls => base.GetNode<Controls>("Controls");
	private Player Player => base.GetNode<Player>("Player");
	private PlayableArea PlayableArea => base .GetNode<PlayableArea>("PlayableArea");

	private Goal Goal => base.GetNodeOrNull<Goal>("../Goal");
	private List<GravityWell> GravityWells => base.GetNodeOrNull<Node>("../GravityWells")
		?.GetChildren()
		.Cast<GravityWell>()
		.ToList();

	public override void _Ready()
	{
		// this.Goal?.SetPlayableArea(polygon: this.PlayableArea.CollisionShape.Polygon);

		this.Goal?.Connect("GoalScored", this, "ScoreGoal");
		this.GravityWells?.ForEach(x => x.Connect("GameOver", this, "EndGame"));

		this.Start();
	}

	private void Start()
	{
		this.Player.Start();
		this.Goal?.Start();
		this.GravityWells?.ForEach(x => x.Start());
		this.PlayableArea.CollisionShape.Disabled = false;
	}

	private void Stop()
	{
		this.Player.Stop();
		this.Goal?.Stop();
		this.GravityWells?.ForEach(x => x.Stop());
		this.PlayableArea.CollisionShape.Disabled = true;
	}

	public void NewGame()
	{
		this.UpdateScore(0);

		this.Start();
	}

	private void UpdateScore(int newScore)
	{
		this.Score = newScore;

		this.Controls.UpdateScore(this.Score);
	}

	public void QuitGame()
	{
		this.Stop();
	}

	public void EndGame()
	{
		this.Player.PlayCrashSound();
		// this.Controls.ShowGameOver();

		this.UpdateScore(0);

		this.Stop();

		base.GetTree().ReloadCurrentScene();
	}

	public void ScoreGoal()
	{
		this.Player.PlayCoinSound();
		this.UpdateScore(this.Score + 1);
		this.Stop();
	}

	public void ZoomIn() => this.Player.Zoom(delta: -1);
	public void ZoomOut() => this.Player.Zoom(delta: 1);
}
