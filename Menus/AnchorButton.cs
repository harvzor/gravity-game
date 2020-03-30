using Godot;
using System;

public class AnchorButton : LinkButton
{
    [Export]
    public string Href;
    public override void _Ready()
    {
        this.Connect("pressed", this, "OnClick");
    }

    public void OnClick() => OS.ShellOpen(this.Href);
}
