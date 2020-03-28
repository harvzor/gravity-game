using Godot;
using System;

public class SceneButton : Button
{
	[Export]
	public PackedScene ConnectingScene;

    public override void _Pressed()
    {
		base.GetTree().ChangeSceneTo(this.ConnectingScene);
    }
}
