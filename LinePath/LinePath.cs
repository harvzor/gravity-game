using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LinePath : Node2D
{
    private float Frame = 0;
    private List<Vector2> Points = new List<Vector2>();
    private const int MaxPoints = 100;

    public override void _PhysicsProcess(float delta)
    {
        if (this.Frame % 3 == 0)
        {
            this.Points.Add(base.GlobalPosition);

            if (this.Points.Count > MaxPoints)
            {
                this.Points.RemoveAt(0);
            }

            this.Frame = 0;
        }

        this.Frame += 1;

        base.Update();
    }

    public override void _Draw()
    {
        if (this.Points.Count < 2)
            return;

        base.DrawSetTransform(new Vector2(0, 0), -((Node2D)base.GetParent()).Rotation, new Vector2(1, 1));
        base.DrawPolylineColors(
            points: this.Points.Select(p => p - this.GlobalPosition).ToArray(),
            // points: this.Points.ToArray(),
            colors: this.Points.Select(p =>
            {
                var color = Color.ColorN("red");

                color.a = (float)this.Points.IndexOf(p) / (float)this.Points.Count();

                return color;
            }).ToArray(),
            width: 3,
            antialiased: false
        );
    }
}
