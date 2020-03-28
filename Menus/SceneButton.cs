using Godot;
using System;

public class SceneButton : Button
{
	[Export]
	public PackedScene ConnectingScene;

  /// <summary>Solves an issue with cyclic dependencies of scenes requiring each other.</summary>
  /// https://www.reddit.com/r/godot/comments/feol0i/cyclic_reference_of_packed_scenes/
	[Export]
	public String ConnectingScenePath;

  public override void _Pressed()
  {
    PlayableArea.IgnoreCollisionsOnce = true;

    var tree = base.GetTree();

    if (this.ConnectingScene != null)
      tree.ChangeSceneTo(this.ConnectingScene);
    else
      tree.ChangeScene(this.ConnectingScenePath);
  }
}
