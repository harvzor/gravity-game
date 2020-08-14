using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Draws a path behind the moving object.
/// </summary>
public class LinePath : Node2D
{
	/// <summary>
	/// If set to true, a new line will be created. New positions will be added to this new line instead of the old one.
	/// </summary>
	public Boolean NewLine = true;
	public Boolean Draw = true;

	// private Timer SecondTimer = new Timer();
	private readonly Timer MillisecondTimer = new Timer();

	// It's a List of List so that there can be different unconnected lines drawn (useful for teleporting the player).
	private readonly List<List<Vector2>> PointGroups = new List<List<Vector2>>();
	private const int MaxPoints = 100;

	public override void _Ready()
	{
		// this.SetupSecondTimer();
		this.SetupMillisecondTimer();
	}

	private void RemoveOldLines()
	{
		if (this.PointGroups.Count > 1)
		{
			this.PointGroups
				.RemoveAll(x => x.Count == 0);
		}
	}

	// private void SetupSecondTimer()
	// {
	//     base.AddChild(this.SecondTimer);

	//     this.SecondTimer.WaitTime = 1;
	//     this.SecondTimer.OneShot = false;

	//     this.SecondTimer.Connect("timeout", this, "RemovePointsOverTime");

	//     this.SecondTimer.Start();
	// }

	private void SetupMillisecondTimer()
	{
		base.AddChild(this.MillisecondTimer);

		this.MillisecondTimer.WaitTime = 0.02f;
		this.MillisecondTimer.OneShot = false;

		this.MillisecondTimer.Connect("timeout", this, nameof(this.Record));

		this.MillisecondTimer.Start();
	}

	// Ensure the line shortens even when not moving.
	private void RemovePointsOverTime()
	{
		foreach (var points in this.PointGroups)
		{
			if (points.Any())
				points.RemoveAt(0);
		}
	}

	public void Record()
	{
		if (this.NewLine)
		{
			this.PointGroups.Add(new List<Vector2>());

			this.NewLine = false;
		}

		var points = this.PointGroups.Last();

		if (this.Draw)
		{
			points.Add(base.GlobalPosition);

			if (points.Count > MaxPoints)
				points.RemoveAt(0);

			this.RemoveOldLines();
		}

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
