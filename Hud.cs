using Godot;
using System;

public class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	private Label MessageLabel => base.GetNode<Label>("MessageLabel");
	private Label ScoreLabel => base.GetNode<Label>("ScoreLabel");
	private Button StartButton => base.GetNode<Button>("StartButton");

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

		// When you need to pause for a brief time, an alternative to using a Timer node is to use the SceneTree’s create_timer() function. This can be very useful to delay, such as in the above code, where we want to wait a little bit of time before showing the “Start” button.

		this.ShowMessage("Gravity!");

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

		base.EmitSignal("StartGame");
	}
}
