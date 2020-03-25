using Godot;
using System;

public class Player : RigidBody2D
{

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	[Export]
	public float ZoomStep = 0.5f;

	private Vector2 InitialPosition;
	private Vector2? NewVelocity;
	private Boolean Reset;
	private Vector2 ScreenSize;
	private Vector2? NewZoom;
	private Vector2 MinZoom = new Vector2(
		x: 0.5f,
		y: 0.5f
	);

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

	private Vector2? DragCurrentPosition;
	private Vector2? DragEndPosition;

	private Node2D Sprite => base.GetNode<Node2D>("Sprite");
	private CollisionShape2D CollisionShape => base.GetNode<CollisionShape2D>("CollisionShape2D");
	private Line Line => base.GetNode<Line>("Line");
	private Camera2D Camera => base.GetNode<Camera2D>("Camera");

	/// <summary>Calculate the firing of this item.</summary>
	private void CalculateVelocityFromMouseDrag()
	{
		Vector2 newVelocity = new Vector2(
			x: this.DragEndPosition.Value.x - this.Position.x,
			y: this.DragEndPosition.Value.y - this.Position.y
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

		base.Show();
		// this.CollisionShape.Disabled = false;
	}

	public void Stop()
	{
		base.Hide();

		this.Reset = true;

		this.Dragging = false;

		// this.CollisionShape.Disabled = true;
	}

	private void Zoom(float delta)
	{
		var newZoom = new Vector2(
			x: this.Camera.Zoom.x + this.ZoomStep * delta,
			y: this.Camera.Zoom.y + this.ZoomStep * delta
		);

		if (newZoom < this.MinZoom)
			newZoom = this.MinZoom;

		this.NewZoom = newZoom;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			var buttonIndex = (ButtonList)mouseEvent.ButtonIndex;

			if (buttonIndex == ButtonList.Left)
			{
				if ((mouseEvent.Position - base.Position).Length() < this.ClickRadius)
				{
					// Start dragging if the click is on the sprite.
					if (!this.Dragging && mouseEvent.Pressed)
					{
						this.Dragging = true;
					}
				}

				// Stop dragging if the button is released.
				if (this.Dragging && !mouseEvent.Pressed)
				{
					this.Dragging = false;
					this.DragEndPosition = mouseEvent.Position;

					this.CalculateVelocityFromMouseDrag();

					this.DragCurrentPosition = null;
					this.DragEndPosition = null;
				}
			}
			else if (buttonIndex == ButtonList.WheelDown)
			{
				this.Zoom(delta: 1);
			}
			else if (buttonIndex == ButtonList.WheelUp)
			{
				this.Zoom(delta: -1);
			}
		}
	}

	public override void _Process(float delta)
	{
		this.Sprite.Rotation =	this.LinearVelocity.Angle() + (float)Math.PI / 2;

		if (this.Dragging)
			this.DragCurrentPosition = base.GetGlobalMousePosition();

		this.Line.DragCurrentPosition = this.DragCurrentPosition;
		this.Line.DragEndPosition = this.DragEndPosition;

		if (this.NewZoom != null && this.NewZoom.Value.x != this.Camera.Zoom.x)
		{
			// Smooth zoom.
			this.Camera.Zoom = this.Camera.Zoom.MoveToward(this.NewZoom.Value, delta * 10);
		}
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (this.Reset)
		{
			this.Reset = false;

			state.LinearVelocity = new Vector2(x: 0, y: 0);

			state.Transform = new Transform2D(rot: 0, pos: this.InitialPosition);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (this.NewVelocity != null)
		{
			this.LinearVelocity += this.NewVelocity.Value;

			this.NewVelocity = null;
		}
	}
}
