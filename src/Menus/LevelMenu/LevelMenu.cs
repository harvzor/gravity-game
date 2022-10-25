using Godot;

public class LevelMenu : ScrollContainer
{
    private Global Global => base.GetNode<Global>("/root/Global");

    public override void _Ready()
    {
        var vBoxContainer= base.GetNode<VBoxContainer>("VBoxContainer");
        var sceneButton = vBoxContainer.GetNode<SceneButton>("SceneButtonTemplate");

        int i = 1;

        foreach (string level in this.Global.LevelService.GetLevels())
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

}
