using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Main : Node
{
	[Export]
	public AudioStream Music;

	private int _Score;
	private int Score
	{
		get => _Score;
		set
		{
			this._Score = value;

			this.Controls.UpdateScore(this.Score);
		}
	}

	private int _MoveCounter;
	private int MoveCounter
	{
		get => _MoveCounter;
		set
		{
			this._MoveCounter = value;

			this.Controls.UpdateMoveCounter(this._MoveCounter);
		}
	}

	private Global Global => base.GetNode<Global>("/root/Global");
	private Controls Controls => base.GetNode<Controls>("Controls");
	private Player Player => base.GetNode<Player>("Player");
	private PlayableArea PlayableArea => base.GetNode<PlayableArea>("PlayableArea");

	private Goal Goal => base.GetNodeOrNull<Goal>("../Goal");
	private List<Collidable> Collideables => base.GetNodeOrNull<Node>("../Collideables")
		?.GetChildren()
		.Cast<Collidable>()
		.ToList();

	public override void _Ready()
	{
		this.Goal?.SetPlayableArea(polygon: this.PlayableArea.CollisionShape.Polygon, scale: this.PlayableArea.Scale);

		this.Goal?.Connect("GoalScored", this, nameof(ScoreGoal));
		this.Collideables?.ForEach(collideable => collideable.Connect("Crash", this, nameof(Crash)));

		this.Start();

		if (this.Goal != null)
			this.Controls.SetNextScene(nextScene: this.Goal.NextScene);
	}

	private void Start()
	{
		this.Player.Start();
		this.Goal?.Start();
		this.Collideables?.ForEach(x => x.Start());
		this.PlayableArea.CollisionShape.Disabled = false;

		if (this.Music != null)
			this.Global.ChangeMusic(this.Music);
	}

	private void Stop()
	{
		this.Player.Stop();
		this.Goal?.Stop();
		this.Collideables?.ForEach(x => x.Stop());
		this.PlayableArea.CollisionShape.SetDeferred("disabled", true);
	}

	public void NewGame()
	{
		this.Score = 0;
		this.MoveCounter = 0;

		this.Start();
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

		this.Score = 0;

		this.Stop();

		// Collisions with the playable area must be stopped before this is ran.
		base.GetTree().ReloadCurrentScene();
	}

	public void ScoreGoal()
	{
		this.Score++;

		if (this.Goal?.MoveOnGoal == false)
			this.Stop();

		if (this.Goal.NextScene != null)
		{
			this.SetHighestLevelUnlocked();

			int points = this.CalculatePoints();

			this.Controls.ShowPoints(
				moves: this.MoveCounter,
				timeFuel: 15,
				time: 25,
				totalScore: 130
			);
		}
	}

	private void SetHighestLevelUnlocked()
	{
		// Pretty hacky.
		var split = this.Goal.NextScene.ResourcePath.Split('/');

		int levelNumber = Int32.Parse(
			split[split.Length - 1]
				.Replace("Level", "")
				.Replace(".tscn", "")
		);

		if (levelNumber > this.Global.HighestLevelUnlocked)
			this.Global.HighestLevelUnlocked = levelNumber;
	}

	private int CalculatePoints()
	{
		return this.Player.TimeFuel / this.MoveCounter;;
	}

	public void OnPlayerMoved()
	{
		this.MoveCounter++;
	}

	public void OnFuelChanged(int newFuelValue)
	{
		this.Controls.UpdateFuel(newFuelValue: newFuelValue);
	}

	private void OnTimeFuelChanged(int newTimeFuelValue)
	{
		this.Controls.UpdateTimeFuel(newTimeFuelValue: newTimeFuelValue);
	}

	public void ZoomIn() => this.Player.Zoom(delta: -1);
	public void ZoomOut() => this.Player.Zoom(delta: 1);
}
