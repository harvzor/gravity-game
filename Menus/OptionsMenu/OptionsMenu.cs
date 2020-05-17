using Godot;
using System;

public class OptionsMenu : VBoxContainer
{
    private Global Global => base.GetNode<Global>("/root/Global");
    private HSlider MusicVolumeSlider => base.GetNode<HSlider>("MusicVolumeSlider");
    private Button ResetButton => base.GetNode<Button>("ResetButton");

    public override void _Ready()
    {
        this.SetupSettings();

        this.ResetButton.Connect("pressed", this, "OnResetButtonClick");
    }

    private void SetupSettings()
    {
        this.MusicVolumeSlider.Value = this.Global.MusicVolume;
    }

    public void MusicVolumeChanged(float newValue)
    {
        this.Global.MusicVolume = (int)newValue;
    }

    public void OnResetButtonClick()
    {
        this.Global.StorageManager.SetDefault();
        this.Global.StorageManager.Save();

        this.SetupSettings();
    }
}
