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

	// private Label MessageLabel => base.GetNode<Label>("MessageLabel");
	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");
	// private Button StartButton => base.GetNode<Button>("StartButton");
	private Button PauseButton => base.GetNode<Button>("PauseButton");
	private SceneButton MainMenuButton => base.GetNode<SceneButton>("MainMenuButton");
	private Button ZoomInButton=> base.GetNode<Button>("ZoomInButton");
	private Button ZoomOutButton => base.GetNode<Button>("ZoomOutButton");

	public override void _Ready()
	{
		this.Start();
	}

	public void Start()
	{
		// this.ShowMessage("Gravity!");

		// this.StartButton.Hide();
		// this.HideMessage();
		this.PauseButton.Show();
		this.ScoreLabel.Show();
		this.ZoomInButton.Show();
		this.ZoomOutButton.Show();
		this.MainMenuButton.Hide();
	}

	public void Stop()
	{
		// this.StartButton.Show();
		this.PauseButton.Hide();
		this.ScoreLabel.Show();
		this.MainMenuButton.Show();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();
	}

	// public void ShowMessage(string text)
	// {
	// 	this.MessageLabel.Text = text;
	// 	this.MessageLabel.Show();
	// }

	// public void HideMessage()
	// {
	// 	this.MessageLabel.Hide();
	// }

	// public void ShowGameOver()
	// {
	// 	this.ShowMessage("Game Over");

	// 	this.StartButton.Show();
	// }

	public void UpdateScore(int score)
	{
		this.ScoreLabel.Text = score.ToString();
	}

	public void OnStartButtonPressed()
	{
		base.EmitSignal("StartGame");
	}

	public void OnPausButtonPressed()
	{
		this.Stop();
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
