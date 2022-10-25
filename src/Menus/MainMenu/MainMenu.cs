using Godot;
using System;

public class MainMenu : CanvasLayer
{
	[Export]
	public AudioStream Music;

    private Global Global => base.GetNode<Global>("/root/Global");

    public override void _Ready()
    {
		if (this.Music != null)
			this.Global.ChangeMusic(this.Music);
    }
}
