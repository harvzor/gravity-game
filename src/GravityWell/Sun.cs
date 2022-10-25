using Godot;
using System;

public class Sun : Collidable
{
    private GravityWell GravityWell => base.GetNode<GravityWell>("GravityWell");

    public override void _Ready()
    {
        // Scale the gravity with the size.
        this.GravityWell.Gravity = this.GravityWell.Gravity * this.Scale.x * this.Scale.x;

        // this.GravityWell.Gravity = 2000;
        // this.GravityWell.Gravity = this.GravityWell.Gravity * this.Gravity;
        // for (var i = 1; i < this.Scale.x; i++)
        // {
        //     var newGravityWell = this.GravityWell.Duplicate();
        //     this.AddChild(newGravityWell);
        // }

        base._Ready();
    }
}
