using Godot;
using System;

public class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	private Label GetMessageLabel() => base.GetNode<Label>("MessageLabel");
	private Label GetScoreLabel() => base.GetNode<Label>("ScoreLabel");
	private Button GetStartButton() => base.GetNode<Button>("StartButton");

	public void ShowMessage(string text)
	{
		var messageLabel = this.GetMessageLabel();

		messageLabel.Text = text;
		messageLabel.Show();
	}

	public void HideMessage()
	{
		var messageLabel = this.GetMessageLabel();

		messageLabel.Hide();
	}

	public void ShowGameOver()
	{
		this.ShowMessage("Game Over");

		// When you need to pause for a brief time, an alternative to using a Timer node is to use the SceneTree’s create_timer() function. This can be very useful to delay, such as in the above code, where we want to wait a little bit of time before showing the “Start” button.

		this.ShowMessage("Gravity!");

		this.GetStartButton().Show();
	}

	public void UpdateScore(int score)
	{
		this.GetScoreLabel().Text = score.ToString();
	}

	public void OnStartButtonPressed()
	{
		this.GetStartButton().Hide();
		this.HideMessage();

		base.EmitSignal("StartGame");
	}
}
