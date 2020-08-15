using Godot;
using System.Collections.Generic;

public class LevelService
{
    private const string LevelsDir = "res://Levels//";

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
}
