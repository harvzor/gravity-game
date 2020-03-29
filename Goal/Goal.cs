using Godot;
using System;
using System.Threading.Tasks;

public class Goal : Area2D
{
	[Signal]
	public delegate void GoalScored();

	[Export]
	public PackedScene NextScene;

	[Export]
	public Boolean MoveOnGoal;

	private Rect2 PlayableArea;

	private Node2D Sprite => base.GetNode<Node2D>("Sprite");
	private Particles2D Death => base.GetNode<Particles2D>("Death");
	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	public override void _Process(float delta)
	{
		base.RotationDegrees = base.RotationDegrees + 180 * delta;
	}

	public void SetPlayableArea(Vector2[] polygon, Vector2 scale, int margin = 100)
	{
		// Note: maybe I could have just used Vector2.Project ?

		var topLeft = polygon[0];
		var bottomRight = polygon[2];

		var center = new Vector2(
			x: (bottomRight.x - topLeft.x) / 2,
			y: (bottomRight.y - topLeft.y) / 2
		);

		var scaledTopLeft = (topLeft - center * scale) + center;
		var scaledBottomRight = (bottomRight + center * scale) - center;

		var playableArea = new Rect2(
			position: new Vector2(
				x: scaledTopLeft.x + margin,
				y: scaledTopLeft.y + margin
			),
			width: scaledBottomRight.x - scaledTopLeft.x - margin * 2,
			height: scaledBottomRight.y - scaledTopLeft.y - margin * 2
		);

		// playableArea = playableArea;

		this.PlayableArea = playableArea;
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		base.Show();

		// this.CollisionShape.SetDeferred("disabled", false);
	}

	public void Stop()
	{
		base.Hide();

		// this.CollisionShape.SetDeferred("disabled", true);
	}

	public async Task Crash()
	{
		this.Sprite.Hide();

		var timer = new Timer()
		{
			WaitTime = this.Death.Lifetime + ((ParticlesMaterial)this.Death.ProcessMaterial).LifetimeRandomness,
			OneShot = true
		};

		this.AddChild(timer);

		timer.Start();

		this.Death.Emitting = true;

		await base.ToSignal(timer, "timeout");

		timer.QueueFree();
	}

	///<summary>Change the location of the Goal to somewhere random.</summary>
	private void MoveRandom()
	{
		Random rnd = new Random();

		var randomX = rnd.Next((int)this.PlayableArea.Position.x, (int)this.PlayableArea.End.x);
		var randomY = rnd.Next((int)this.PlayableArea.Position.y, (int)this.PlayableArea.End.y);

		base.Position = new Vector2(
			x: randomX,
			y: randomY
		);
	}

	public async void OnGoalBodyEntered(KinematicBody2D body)
	{
		if (body.Name == "Player")
		{
			await this.Crash();

			base.EmitSignal("GoalScored");


			if (this.MoveOnGoal)
			{
				this.Sprite.Show();

				this.MoveRandom();
			}

			if (this.NextScene != null)
				base.GetTree().ChangeSceneTo(this.NextScene);
		}
	}
}
