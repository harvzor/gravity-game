using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Main : Node
{
	[Export]
	public AudioStream Music;

	private int Score;

    private Global Global => base.GetNode<Global>("/root/Global");
	private Controls Controls => base.GetNode<Controls>("Controls");
	private Player Player => base.GetNode<Player>("Player");
	private PlayableArea PlayableArea => base.GetNode<PlayableArea>("PlayableArea");

	private Goal Goal => base.GetNodeOrNull<Goal>("../Goal");
	private List<Collidable> GravityWells => base.GetNodeOrNull<Node>("../GravityWells")
		?.GetChildren()
		.Cast<Collidable>()
		.ToList();

	public override void _Ready()
	{
		this.Goal?.SetPlayableArea(polygon: this.PlayableArea.CollisionShape.Polygon, scale: this.PlayableArea.Scale);

		this.Goal?.Connect("GoalScored", this, "ScoreGoal");
		this.GravityWells?.ForEach(gravityWell => gravityWell.Connect("Crash", this, "Crash"));

		this.Start();
	}

	private void Start()
	{
		this.Player.Start();
		this.Goal?.Start();
		this.GravityWells?.ForEach(x => x.Start());
		this.PlayableArea.CollisionShape.Disabled = false;

		if (this.Music != null)
			this.Global.ChangeMusic(this.Music);
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

	public async void Crash()
	{
		// this.Player.Crash();
		await this.Player.Crash();

		// this.Controls.ShowGameOver();

		this.UpdateScore(0);

		this.Stop();

		// Collisions with the playable area must be stopped before this is ran.
		base.GetTree().ReloadCurrentScene();
	}

	public void ScoreGoal()
	{
		this.UpdateScore(this.Score + 1);

		if (this.Goal?.MoveOnGoal == false)
			this.Stop();

		if (this.Goal.NextScene != null)
			this.Controls.ShowLevelComplete(nextScene: this.Goal.NextScene);
	}

	public void OnFuelChanged(int newFuelValue)
	{
		this.Controls.UpdateFuel(newFuelValue: newFuelValue);
	}

	public void ZoomIn() => this.Player.Zoom(delta: -1);
	public void ZoomOut() => this.Player.Zoom(delta: 1);
}
