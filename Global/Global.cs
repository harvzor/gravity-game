using Godot;
using System;

public class Global : Node
{
	public AudioStreamPlayer CrashSound => base.GetNode<AudioStreamPlayer>("Sound/Crash");
	public AudioStreamPlayer Coin => base.GetNode<AudioStreamPlayer>("Sound/Coin");
	public AudioStreamPlayer Music => base.GetNode<AudioStreamPlayer>("Music");

    public override void _Ready()
    {
    }

    public void ChangeMusic(AudioStream audioStream)
    {
        if (this.Music.Stream != null && audioStream.ResourcePath == this.Music.Stream.ResourcePath)
            return;

        this.Music.Stream = audioStream;

        if (this.Music.Playing == false)
            this.Music.Play();
    }
}
