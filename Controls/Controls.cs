using Godot;
using System;

public class Controls : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	[Signal]
	public delegate void ZoomIn();

	[Signal]
	public delegate void ZoomOut();

	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");

	private HBoxContainer FuelContainer => base.GetNode<HBoxContainer>("FuelContainer");
	private ProgressBar FuelBar => this.FuelContainer.GetNode<ProgressBar>("FuelBar");
	private Button PauseButton => base.GetNode<Button>("PauseButton");
	private Control MenuContainer => base.GetNode<Control>("MenuContainer");
	private Button ZoomInButton=> base.GetNode<Button>("ZoomInButton");
	private Button ZoomOutButton => base.GetNode<Button>("ZoomOutButton");

	private Node2D LevelComplete => base.GetNode<Node2D>("LevelComplete");
	private SceneButton ContinueButton => this.LevelComplete.GetNode<SceneButton>("MenuContainer/ContinueButton");

	public override void _Ready()
	{
		this.Start();
	}

	public void Start()
	{
		this.PauseButton.Show();
		this.MenuContainer.Hide();
		this.ZoomInButton.Show();
		this.ZoomOutButton.Show();
	}

	public void Stop()
	{
		this.PauseButton.Hide();
		this.MenuContainer.Show();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
	}

	public void UpdateFuel(int newFuelValue)
	{
		this.FuelBar.Value = newFuelValue;
	}

	public void UpdateScore(int score)
	{
		this.ScoreLabel.Text = score.ToString();
	}

	public void ShowLevelComplete(PackedScene nextScene)
	{
		this.PauseButton.Hide();
		this.MenuContainer.Hide();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
		this.FuelContainer.Hide();
		this.LevelComplete.Show();

		this.ContinueButton.ConnectingScene = nextScene;
	}

	public void OnStartButtonPressed()
	{
		this.GetTree().Paused = false;

		base.EmitSignal("StartGame");
	}

	public void OnRetryButtonPressed()
	{
		this.GetTree().Paused = false;

		this.GetTree().ReloadCurrentScene();
	}

	public void OnPauseButtonPressed()
	{
		this.Stop();

		this.GetTree().Paused = true;
	}

	public void OnResumeButtonPressed()
	{
		this.Start();

		this.GetTree().Paused = false;
	}

	public void OnZoomInButtonPressed()
	{
		base.EmitSignal("ZoomIn");
	}

	public void OnZoomOutButtonPressed()
	{
		base.EmitSignal("ZoomOut");
	}
}
