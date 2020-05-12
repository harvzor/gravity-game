using Godot;
using System;
using System.Collections.Generic;

public class LevelMenu : ScrollContainer
{
    private const string LevelsDir = "res://Levels//";

    private Global Global => base.GetNode<Global>("/root/Global");

    public override void _Ready()
    {
        var vBoxContainer= base.GetNode<VBoxContainer>("VBoxContainer");
        var sceneButton = vBoxContainer.GetNode<SceneButton>("SceneButtonTemplate");

        int i = 1;

        foreach (string level in this.GetLevels())
        {
            var newButton = (SceneButton)sceneButton.Duplicate();

            // Level lock
            newButton.Disabled = i > this.Global.HighestLevelUnlocked;

            newButton.Text = "Level " + i;
            // newButton.ConnectingScene = level;
            newButton.ConnectingScenePath = level;
            newButton.Show();

            vBoxContainer.AddChild(newButton);

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
