using Godot;
using System;

public class Global : Node
{
	public AudioStreamPlayer CrashSound => base.GetNode<AudioStreamPlayer>("Sound/Crash");
	public AudioStreamPlayer Coin => base.GetNode<AudioStreamPlayer>("Sound/Coin");

    public override void _Ready()
    {

    }
}
