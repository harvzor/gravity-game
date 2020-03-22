using Godot;
using System;

public class Player : RigidBody2D
{
	[Signal]
	public delegate void Goal();

	[Signal]
	public delegate void GameOver();

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	private Boolean Reset;
	private Vector2 ScreenSize;
	private Boolean Dragging = false;
	private Vector2 DragStartPosition;
	private Vector2 DragEndPosition;

	private AnimatedSprite Sprite => base.GetNode<AnimatedSprite>("AnimatedSprite");
	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");

	/// <summary>Calculate the firing of this item.</summary>
	private void CalculateVelocityFromMouseDrag()
	{
		Vector2 newVelocity = new Vector2(
			x: this.DragEndPosition.x - this.DragStartPosition.x,
			y: this.DragEndPosition.y - this.DragStartPosition.y
		);

		if (newVelocity.Length() > 0)
		{
			newVelocity = newVelocity * this.Speed;
		}

		base.LinearVelocity = newVelocity;
	}

	public override void _Ready()
	{
		this.ScreenSize = base.GetViewport().Size;

		this.Stop();
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		this.Dragging = false;

		this.Reset = true;

		base.Position = new Vector2(
			x: this.ScreenSize.x / 2,
			y: this.ScreenSize.y / 2
		);

		base.Show();
		this.CollisionShape.Disabled = false;
	}

	public void Stop()
	{
		base.Hide();

		this.Dragging = false;

		this.CollisionShape.Disabled = true;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
		{
			if ((mouseEvent.Position - base.Position).Length() < this.ClickRadius)
			{
				// Start dragging if the click is on the sprite.
				if (!this.Dragging && mouseEvent.Pressed)
				{
					this.Dragging = true;
					this.DragStartPosition = mouseEvent.Position;
				}
			}

			// Stop dragging if the button is released.
			if (this.Dragging && !mouseEvent.Pressed)
			{
				this.Dragging = false;
				this.DragEndPosition = mouseEvent.Position;

				this.CalculateVelocityFromMouseDrag();
			}
		}
		// else if (inp
	}

	public override void _Process(float delta)
	{
		if (this.LinearVelocity.Length() > 0)
			this.Sprite.Play();
		else
			this.Sprite.Stop();


		this.Sprite.Rotation =	this.LinearVelocity.Angle() + (float)Math.PI / 2;
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (this.Reset)
		{
			base.LinearVelocity=  new Vector2(x: 0, y: 0);

			base.Position = new Vector2(
				x: this.ScreenSize.x / 2,
				y: this.ScreenSize.y / 2
			);

			this.Reset = false;
		}
	}

	public void OnPlayerAreaEntered(Area2D area)
	{
		if (area.Name == "Goal")
			base.EmitSignal("Goal");
	}

	public void OnPlayerAreaExited(Area2D area)
	{
		if (area.Name == "PlayableArea")
			base.EmitSignal("GameOver");
	}
}
