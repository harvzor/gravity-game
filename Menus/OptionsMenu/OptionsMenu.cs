using Godot;
using System;

public class OptionsMenu : VBoxContainer
{
    private Global Global => base.GetNode<Global>("/root/Global");
    private HSlider MusicVolumeSlider => base.GetNode<HSlider>("MusicVolumeSlider");

    public override void _Ready()
    {

    }

    public void MusicVolumeChanged(float newValue)
    {
        this.Global.Music.VolumeDb = this.CalcualteDecibels(percentage: newValue);
    }

    private float CalcualteDecibels(float percentage)
    {
        // Minimum can be -80.

        if (percentage < 1)
            return -80;

        return 0.4f * (percentage - 100);
    }
}
