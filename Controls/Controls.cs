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

	private Global Global => base.GetNode<Global>("/root/Global");

	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");

	private HBoxContainer FuelContainer => base.GetNode<HBoxContainer>("FuelContainer");
	private ProgressBar FuelBar => this.FuelContainer.GetNode<ProgressBar>("FuelBar");
	private HBoxContainer TimeFuelContainer => base.GetNode<HBoxContainer>("TimeFuelContainer");
	private ProgressBar TimeFuelBar => this.TimeFuelContainer.GetNode<ProgressBar>("TimeFuelBar");
	private Button PauseButton => base.GetNode<Button>("PauseButton");
	private Control MenuContainer => base.GetNode<Control>("MenuContainer");
	private Button ZoomInButton=> base.GetNode<Button>("ZoomInButton");
	private Button ZoomOutButton => base.GetNode<Button>("ZoomOutButton");

	private CanvasItem LevelComplete => base.GetNode<CanvasItem>("LevelComplete");
	private Label LevelCompleteText => this.LevelComplete.GetNode<Label>("PointsContainer/LevelCompleteText");
	private SceneButton ContinueButton => this.LevelComplete.GetNode<SceneButton>("MenuContainer/ContinueButton");

	private Control Curtain => this.GetNode<Control>("Curtain");

	public override void _Ready()
	{
		this.Start();

		this.LevelCompleteText.Text = this.LevelCompleteText.Text.Replace("#", this.Global.LevelService.GetCurrentLevel().ToString());
	}

	public void Start()
	{
		this.Curtain.Hide();
		this.PauseButton.Show();
		this.MenuContainer.Hide();
		this.ZoomInButton.Show();
		this.ZoomOutButton.Show();
	}

	public void Stop()
	{
		this.Curtain.Show();
		this.PauseButton.Hide();
		this.MenuContainer.Show();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
	}

	public void UpdateFuel(int newFuelValue)
	{
		this.FuelBar.Value = newFuelValue;
	}

	public void UpdateTimeFuel(int newTimeFuelValue)
	{
		this.TimeFuelBar.Value = newTimeFuelValue;
	}

	public void UpdateScore(int score)
	{
		this.ScoreLabel.Text = score.ToString();
	}

	public void ShowLevelComplete(PackedScene nextScene)
	{
		this.Curtain.Show();
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
