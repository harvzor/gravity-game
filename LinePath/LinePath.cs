using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LinePath : Node2D
{
    public Boolean NewLine = true;
    public Boolean Draw = true;

    private float Frame = 0;

    // It's a List of List so that there can be different unconnected lines drawn (useful for teleporting the player).
    private List<List<Vector2>> PointGroups = new List<List<Vector2>>();
    private const int MaxPoints = 100;

    private void RemoveOldLines()
    {
        if (this.PointGroups.Count > 1)
        {
            this.PointGroups
                .RemoveAll(x => x.Count == 0);
        }
    }

    // Ensure the line shortens even when not moving.
    private void RemovePointsOverTime()
    {
        if (this.Frame % 3 == 0)
        {
            foreach (var points in this.PointGroups)
            {
                if (points.Any())
                    points.RemoveAt(0);

                if (points.Any())
                    points.RemoveAt(0);
            }

            this.Frame = 0;
        }

        this.Frame += 1;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (this.NewLine)
        {
            this.PointGroups.Add(new List<Vector2>());

            this.NewLine = false;
        }

        var points = this.PointGroups.Last();

        if (this.Draw)
            points.Add(base.GlobalPosition);

        if (points.Count > MaxPoints)
        {
            points.RemoveAt(0);
        }

        this.RemovePointsOverTime();

        if (this.Draw)
            this.RemoveOldLines();

        base.Update();
    }

    public override void _Draw()
    {
        foreach (var points in this.PointGroups)
        {
            if (points.Count < 2)
                return;

            base.DrawSetTransform(new Vector2(0, 0), -((Node2D)base.GetParent()).Rotation, new Vector2(1, 1));
            base.DrawPolylineColors(
                points: points.Select(p => p - this.GlobalPosition).ToArray(),
                // points: this.Points.ToArray(),
                colors: points.Select(p =>
                {
                    var color = Color.ColorN("white");

                    var alpha = ((float)points.IndexOf(p) / (float)points.Count()) - 0.3f;

                    color.a = alpha >= 0
                        ? alpha
                        : 0;

                    return color;
                }).ToArray(),
                width: 16,
                antialiased: false
            );
        }
    }
}
