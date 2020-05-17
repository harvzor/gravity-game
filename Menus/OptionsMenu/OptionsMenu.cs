using Godot;
using System;

public class OptionsMenu : VBoxContainer
{
    private Global Global => base.GetNode<Global>("/root/Global");
    private HSlider MusicVolumeSlider => base.GetNode<HSlider>("MusicVolumeSlider");

    public override void _Ready()
    {
        this.MusicVolumeSlider.Value = this.Global.MusicVolume;
    }

    public void MusicVolumeChanged(float newValue)
    {
        this.Global.MusicVolume = (int)newValue;
    }
}
