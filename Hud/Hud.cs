using Godot;
using System;

public class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	[Signal]
	public delegate void QuitGame();

	[Signal]
	public delegate void ZoomIn();

	[Signal]
	public delegate void ZoomOut();

	private Label MessageLabel => base.GetNode<Label>("MessageLabel");
	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");
	private Button StartButton => base.GetNode<Button>("StartButton");
	private Button ExitButton => base.GetNode<Button>("ExitButton");
	private Button ZoomInButton=> base.GetNode<Button>("ZoomInButton");
	private Button ZoomOutButton => base.GetNode<Button>("ZoomOutButton");

	public override void _Ready()
	{
		this.MainMenu();
	}

	public void MainMenu()
	{
		this.StartButton.Show();
		this.ExitButton.Hide();
		this.ScoreLabel.Show();
		this.ZoomInButton.Hide();
		this.ZoomOutButton.Hide();

		this.ShowMessage("Gravity!");
	}

	public void ShowMessage(string text)
	{
		this.MessageLabel.Text = text;
		this.MessageLabel.Show();
	}

	public void HideMessage()
	{
		this.MessageLabel.Hide();
	}

	public void ShowGameOver()
	{
		this.ShowMessage("Game Over");

		this.StartButton.Show();
	}

	public void UpdateScore(int score)
	{
		this.ScoreLabel.Text = score.ToString();
	}

	public void OnStartButtonPressed()
	{
		this.StartButton.Hide();
		this.HideMessage();
		this.ExitButton.Show();
		this.ScoreLabel.Show();
		this.ZoomInButton.Show();
		this.ZoomOutButton.Show();

		base.EmitSignal("StartGame");
	}

	public void OnExitButtonPressed()
	{
		this.MainMenu();

		base.EmitSignal("QuitGame");
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
