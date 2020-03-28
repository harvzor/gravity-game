using Godot;
using System;

public class Main : Node
{
	private int Score;

	private Hud Hud => base.GetNode<Hud>("Hud");
	private Player Player => base.GetNode<Player>("Player");
	private PlayableArea PlayableArea => base .GetNode<PlayableArea>("PlayableArea");

	private Goal Goal => base.GetNodeOrNull<Goal>("../Goal");
	private GravityWell GravityWell => base.GetNodeOrNull<GravityWell>("../GravityWell");

	public override void _Ready()
	{
		// this.Goal?.SetPlayableArea(polygon: this.PlayableArea.CollisionShape.Polygon);

		this.Player.Stop();
		this.Goal?.Stop();
		this.GravityWell?.Stop();

		this.Goal?.Connect("GoalScored", this, "ScoreGoal");
		this.GravityWell?.Connect("GameOver", this, "EndGame");
	}

	private void Start()
	{
		this.Player.Start();
		this.Goal?.Start();
		this.GravityWell?.Start();
		this.PlayableArea.CollisionShape.Disabled = false;
	}

	private void Stop()
	{
		this.Player.Stop();
		this.Goal?.Stop();
		this.GravityWell?.Stop();
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

		this.Hud.UpdateScore(this.Score);
	}

	public void QuitGame()
	{
		this.Stop();
	}

	public void EndGame()
	{
		this.Player.PlayCrashSound();
		this.Hud.ShowGameOver();

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
