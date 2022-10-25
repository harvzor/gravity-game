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
	private Label MoveCounterLabel => base.GetNode<Label>("MoveCounterLabel");

	private HBoxContainer FuelContainer => base.GetNode<HBoxContainer>("FuelContainer");
	private ProgressBar FuelBar => this.FuelContainer.GetNode<ProgressBar>("FuelBar");
	private HBoxContainer TimeFuelContainer => base.GetNode<HBoxContainer>("TimeFuelContainer");
	private ProgressBar TimeFuelBar => this.TimeFuelContainer.GetNode<ProgressBar>("TimeFuelBar");
	private Button PauseButton => base.GetNode<Button>("PauseButton");
	private Control MenuContainer => base.GetNode<Control>("MenuContainer");
	private Button ZoomInButton=> base.GetNode<Button>("ZoomInButton");
	private Button ZoomOutButton => base.GetNode<Button>("ZoomOutButton");

	private CanvasItem PointsWrapper => base.GetNode<CanvasItem>("PointsWrapper");
		private CanvasItem PointsMenuContainer => this.PointsWrapper.GetNode<CanvasItem>("MenuContainer");
			private CanvasItem PointsContainer => this.PointsMenuContainer.GetNode<CanvasItem>("PointsContainer");
				private CanvasItem Scores => this.PointsContainer.GetNode<CanvasItem>("Scores");
					private Label MovesCountLabel => this.Scores.GetNode<Label>("MovesCountLabel");
					private Label TimeFuelCountLabel => this.Scores.GetNode<Label>("TimeFuelCountLabel");
					private Label TimeCountLabel => this.Scores.GetNode<Label>("TimeCountLabel");
					private Label TotalScoreCountLabel => this.Scores.GetNode<Label>("TotalScoreCountLabel");
		private CanvasItem PointsContinueButton => this.PointsMenuContainer.GetNode<CanvasItem>("ContinueButton");

	private CanvasItem LevelComplete => base.GetNode<CanvasItem>("LevelComplete");
	private Label LevelCompleteText => this.LevelComplete.GetNode<Label>("PointsContainer/LevelCompleteText");
	private Label PointsLabel => this.LevelComplete.GetNode<Label>("PointsContainer/PointsLabel");
	private SceneButton ContinueButton => this.LevelComplete.GetNode<SceneButton>("MenuContainer/ContinueButton");

	private Control Curtain => this.GetNode<Control>("Curtain");

	public override void _Ready()
	{
		this.Start();

		this.LevelCompleteText.Text = this.LevelCompleteText.Text.Replace("#", this.Global.LevelService.GetCurrentLevel().ToString());
        this.PointsContinueButton.Connect("pressed", this, nameof(OnPointsContinueButtonPressed));
	}

	public void Start()
	{
		this.Curtain.Hide();
		this.PauseButton.Show();
		this.MenuContainer.Hide();
		this.ZoomInButton.Show();
		this.ZoomOutButton.Show();
		this.MoveCounterLabel.Show();
	}

	public void Stop()
	{
		this.Curtain.Show();
		this.PauseButton.Hide();
		this.MenuContainer.Show();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
		this.MoveCounterLabel.Show();
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

	public void UpdateMoveCounter(int counter)
	{
		this.MoveCounterLabel.Text = counter.ToString();
	}

	public void ShowPoints(int moves, int timeFuel, TimeSpan time, int totalScore)
	{
		this.Curtain.Show();

		this.PauseButton.Hide();
		this.MenuContainer.Hide();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
		this.FuelContainer.Hide();

		this.MovesCountLabel.Text = moves.ToString();
		this.TimeFuelCountLabel.Text = timeFuel.ToString();
		this.TimeCountLabel.Text = time.ToString("m':'ss");
		this.TotalScoreCountLabel.Text = totalScore.ToString();

		this.PointsWrapper.Show();

		// this.PointsLabel.Text = this.PointsLabel.Text.Replace("#", points.ToString());
	}

	public void SetNextScene(PackedScene nextScene)
	{
		this.ContinueButton.ConnectingScene = nextScene;
	}

	public void ShowLevelComplete()
	{
		this.Curtain.Show();
		this.LevelComplete.Show();
	}

	public void OnPointsContinueButtonPressed()
	{
		this.PointsWrapper.Hide();
		this.ShowLevelComplete();
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
