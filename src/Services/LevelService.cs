using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LevelService : Node
{
    private const string LevelsDir = "res://Levels//";
	private Node CurrentLevel => base.GetNodeOrNull("/root/Level");

    public IEnumerable<string> GetLevels()
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

    public int GetCurrentLevel()
    {
        if (this.CurrentLevel == null)
            return -1;

        // Could be res://Levels/Level01.tscn
        string fileDir = this.CurrentLevel.Filename;

        string fileName = fileDir.Split('/').Reverse().First();
        string levelName = fileName.Split('.').First();

        return Int32.Parse(levelName.Replace("Level", ""));
    }
}
