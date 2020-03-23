using Godot;
using System;

public class Player : RigidBody2D
{

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	private Boolean Reset;
	private Vector2 ScreenSize;
	private Boolean Dragging = false;
	private Vector2? DragStartPosition;
	private Vector2? DragCurrentPosition;
	private Vector2? DragEndPosition;
	private Vector2? NewPosition;

	private PlayerRoot PlayerRoot => base.GetNode<PlayerRoot>("../");
	private AnimatedSprite Sprite => base.GetNode<AnimatedSprite>("AnimatedSprite");
	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");
	private Line Line => base.GetNode<Line>("../Line");

	/// <summary>Calculate the firing of this item.</summary>
	private void CalculateVelocityFromMouseDrag()
	{
		Vector2 newVelocity = new Vector2(
			x: this.DragEndPosition.Value.x - this.DragStartPosition.Value.x,
			y: this.DragEndPosition.Value.y - this.DragStartPosition.Value.y
		);

		if (newVelocity.Length() > 0)
		{
			newVelocity = newVelocity * this.Speed;
		}

		base.LinearVelocity = newVelocity;
	}

	public void SetNewPosition(Vector2 position)
	{
		this.NewPosition = position;

		base.Position = position;
	}

	public override void _Ready()
	{
		this.ScreenSize = base.GetViewport().Size;

		this.Start();
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		this.Dragging = false;

		// this.Reset = true;

		// base.Position = new Vector2(
		// 	x: this.ScreenSize.x / 2,
		// 	y: this.ScreenSize.y / 2
		// );

		base.Show();
		this.CollisionShape.Disabled = false;
	}

	public void Stop()
	{
		base.Hide();

		this.Reset = true;

		this.Dragging = false;

		this.CollisionShape.Disabled = true;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			if ((ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
			{
				if ((mouseEvent.Position - base.Position).Length() < this.ClickRadius)
				{
					// Start dragging if the click is on the sprite.
					if (!this.Dragging && mouseEvent.Pressed)
					{
						this.Dragging = true;
						this.DragStartPosition = base.Position; // mouseEvent.Position;
					}
				}

				// Stop dragging if the button is released.
				if (this.Dragging && !mouseEvent.Pressed)
				{
					this.Dragging = false;
					this.DragEndPosition = mouseEvent.Position;

					this.CalculateVelocityFromMouseDrag();

					this.DragStartPosition = null;
					this.DragCurrentPosition = null;
					this.DragEndPosition = null;
				}
			}
		}
	}

	public override void _Process(float delta)
	{
		if (this.LinearVelocity.Length() > 0)
			this.Sprite.Play();
		else
			this.Sprite.Stop();


		this.Sprite.Rotation =	this.LinearVelocity.Angle() + (float)Math.PI / 2;

		if (this.Dragging)
			this.DragCurrentPosition = base.GetGlobalMousePosition();

		this.Line.DragStartPosition = this.DragStartPosition;
		this.Line.DragCurrentPosition = this.DragCurrentPosition;
		this.Line.DragEndPosition = this.DragEndPosition;
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		// if (this.NewPosition != null)
		// {
		// 	base.Position = this.NewPosition.Value;

		// 	this.NewPosition = null;
		// }

		if (this.Reset)
		{
			base.LinearVelocity=  new Vector2(x: 0, y: 0);

			// base.Position = new Vector2(
			// 	x: this.ScreenSize.x / 2,
			// 	y: this.ScreenSize.y / 2
			// );

			this.Reset = false;
		}
	}

}
