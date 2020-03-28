using Godot;
using System;

public class LevelMenu : CanvasLayer
{
	[Signal]
	public delegate void StartGame();

	[Export]
	public PackedScene StartScene;

	private Button StartButton => base.GetNode<Button>("StartButton");

	public void OnStartButtonPressed()
	{
		base.GetTree().ChangeSceneTo(this.StartScene);
	}
}
