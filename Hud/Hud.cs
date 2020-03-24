using Godot;
using System;

public class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	[Signal]
	public delegate void QuitGame();

	private Label MessageLabel => base.GetNode<Label>("MessageLabel");
	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");
	private Button StartButton => base.GetNode<Button>("StartButton");
	private Button ExitButton => base.GetNode<Button>("ExitButton");

	public override void _Ready()
	{
		this.ExitButton.Hide();
		this.ScoreLabel.Hide();
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

		base.EmitSignal("StartGame");
	}

	public void OnExitButtonPressed()
	{
		this.StartButton.Show();
		this.ExitButton.Hide();
		this.ScoreLabel.Show();

		this.ShowMessage("Gravity!");

		base.EmitSignal("QuitGame");
	}
}
