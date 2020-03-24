using Godot;
using System;

public class Player : RigidBody2D
{

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	private Vector2 InitialPosition;
	private Vector2? NewVelocity;
	private Boolean Reset;
	private Vector2 ScreenSize;

	private Boolean _Dragging;
	private Boolean Dragging
	{
		get => _Dragging;
		set
		{
			this._Dragging = value;
			this.Line.Dragging = value;
		}
	}

	private Vector2? DragStartPosition;
	private Vector2? DragCurrentPosition;
	private Vector2? DragEndPosition;

	private AnimatedSprite Sprite => base.GetNode<AnimatedSprite>("AnimatedSprite");
	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");
	private Line Line => base.GetNode<Line>("Line");

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

		this.NewVelocity = newVelocity;
	}

	public override void _Ready()
	{
		this.InitialPosition = base.Position;

		this.ScreenSize = base.GetViewport().Size;

		this.Start();
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		this.Dragging = false;

		this.Reset = true;

		// base.Position = this.InitialPosition;

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

		this.Line.DragCurrentPosition = this.DragCurrentPosition;
		this.Line.DragEndPosition = this.DragEndPosition;
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (this.Reset)
		{
			this.Reset = false;

			state.LinearVelocity=  new Vector2(x: 0, y: 0);

			state.Transform = new Transform2D(rot: 0, pos: this.InitialPosition);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (this.NewVelocity != null)
		{
			this.LinearVelocity = this.NewVelocity.Value;

			this.NewVelocity = null;
		}
	}
}
