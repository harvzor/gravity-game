using Godot;
using System;
using System.Collections.Generic;

public class LevelMenu : Control
{
    private const string LevelsDir = "res://Levels//";

    public override void _Ready()
    {
        var sceneButton = base.GetNode<SceneButton>("SceneButtonTemplate");

        int i = 1;

        foreach (string level in this.GetLevels())
        {
            var newButton = (SceneButton)sceneButton.Duplicate();

            newButton.Text = "Level " + i;
            // newButton.ConnectingScene = level;
            newButton.ConnectingScenePath = level;
            newButton.Show();

            base.AddChild(newButton);

            i++;
        }
    }

    private IEnumerable<string> GetLevels()
    {
        var dir = new Directory();
        dir.Open(LevelsDir);

        dir.ListDirBegin();

        while (true)
        {
            var file = dir.GetNext();

            if (file == "")
                break;

            if (file.BeginsWith("Level"))
            {
                // var packedScene = new PackedScene();

                // packedScene.ResourceName = this.LevelsDir + file;

                // yield return packedScene;

                yield return LevelsDir + file;
            }
        }
    }
}
