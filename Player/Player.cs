using Godot;
using System;
using System.Threading.Tasks;

public class Player : RigidBody2D
{
	[Signal]
	public delegate void FuelUsed(int newFuelValue);

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	[Export]
	public float ZoomStep = 0.5f;

	private Vector2 InitialPosition;
	private float InitialRotation;
	private Vector2? NewVelocity;
	private Boolean ShouldReset;
	private Boolean ShouldStopMoving;
	private Boolean ShouldSleep;
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

	private Int32 Fuel;

	private Particles2D Smoke => base.GetNode<Particles2D>("Smoke");
	private Particles2D Death => base.GetNode<Particles2D>("Death");

	private Node2D Sprite => base.GetNode<Node2D>("Sprite");
	private CollisionPolygon2D CollisionShape => base.GetNode<CollisionPolygon2D>("CollisionShape2D");
	private Line Line => base.GetNode<Line>("Line");
	private Camera2D Camera => base.GetNode<Camera2D>("Camera");
	private AudioStreamPlayer CrashSound => base.GetNode<AudioStreamPlayer>("Sound/Crash");
	private AudioStreamPlayer Coin => base.GetNode<AudioStreamPlayer>("Sound/Coin");

	/// <summary>Calculate the firing of this item.</summary>
	private Vector2 CalculateVelocityFromMouseDrag()
	{
		Vector2 newVelocity = new Vector2(
			x: this.DragEndPosition.Value.x - this.Position.x,
			y: this.DragEndPosition.Value.y - this.Position.y
		);

		if (newVelocity.Length() > 0)
		{
			newVelocity = newVelocity * this.Speed;
		}

		return newVelocity;
	}

	private void UseFuelFromVelocity(Vector2 velocity)
	{
		var fuelUsage = this.CalculateFuelUsage(velocity: velocity);

		if (fuelUsage > this.Fuel)
		{
			velocity = velocity / fuelUsage * this.Fuel;

			fuelUsage = this.Fuel;
		}

		this.ChangeFuelBy(changeBy: -fuelUsage);

		this.NewVelocity = velocity;
	}

	private void ChangeFuelBy(int changeBy)
	{
		this.Fuel += changeBy;

		this.EmitSignal("FuelUsed", this.Fuel);
	}

	private Int32 CalculateFuelUsage(Vector2 velocity)
	{
		return (int)Math.Round(velocity.Length() / 100);
	}

	public override void _Ready()
	{
		this.InitialPosition = base.Position;
		this.InitialRotation = base.Rotation;

		this.ScreenSize = base.GetViewport().Size;

		this.Start();

		this.ShouldSleep = true;

		this.ChangeFuelBy(100);
	}

	/// <summary>Reset when starting a new game.</summary>
	public void Start()
	{
		this.Dragging = false;

		base.Show();

		this.CollisionShape.SetDeferred("disabled", false);
	}

	public void Stop()
	{
		base.Hide();

		this.ShouldReset = true;
		this.ShouldSleep = true;

		this.Dragging = false;

		this.Smoke.Restart();

		this.CollisionShape.SetDeferred("disabled", true);
	}

	public void Zoom(float delta)
	{
		var newZoom = new Vector2(
			x: this.Camera.Zoom.x + this.ZoomStep * delta,
			y: this.Camera.Zoom.y + this.ZoomStep * delta
		);

		if (newZoom < this.MinZoom)
			newZoom = this.MinZoom;

		this.NewZoom = newZoom;
	}

	public async Task Crash()
	{
		this.Sprite.Hide();

		this.ShouldStopMoving = true;

		this.Smoke.Emitting = false;
		this.CrashSound.Play();

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

	public void PlayCoinSound()
	{
		this.Coin.Play();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			var buttonIndex = (ButtonList)mouseEvent.ButtonIndex;

			if (buttonIndex == ButtonList.Left)
			{
				// Make sure it's stil easy to click even when zoomed out.
				var zoomedClickRadius = this.ClickRadius * this.Camera.Zoom.x;

				if ((base.GetGlobalMousePosition() - base.Position).Length() < zoomedClickRadius)
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
					this.DragEndPosition = base.GetGlobalMousePosition();

					var newVelocity = this.CalculateVelocityFromMouseDrag();

					this.UseFuelFromVelocity(newVelocity);

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

		// Zoom controls via trackpad.
		if (inputEvent is InputEventMagnifyGesture magnifyGesture)
		{
			this.Zoom(delta: magnifyGesture.Factor);
		}
	}

	public override void _Process(float delta)
	{
		if (this.Dragging)
			this.DragCurrentPosition = base.GetGlobalMousePosition();

		this.Line.DragCurrentPosition = this.DragCurrentPosition;

		if (this.NewZoom != null && this.NewZoom.Value.x != this.Camera.Zoom.x)
		{
			// Smooth zoom.
			this.Camera.Zoom = this.Camera.Zoom.MoveToward(this.NewZoom.Value, delta * 10);
		}

		// Smoke should fire in opposite direction of player.
		((ParticlesMaterial)this.Smoke.ProcessMaterial).Direction = new Vector3(
			x: -this.LinearVelocity.x,
			y: -this.LinearVelocity.y,
			z: 0
		);
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (this.ShouldStopMoving)
		{
			this.ShouldStopMoving = true;

			state.LinearVelocity = new Vector2(x: 0, y: 0);
		}

		if (this.ShouldReset)
		{
			this.ShouldReset = false;

			state.LinearVelocity = new Vector2(x: 0, y: 0);

			state.Transform = new Transform2D(rot: this.InitialRotation, pos: this.InitialPosition);
		}
		else
		{
			if (this.ShouldSleep)
			{
				this.ShouldSleep = false;
				this.Sleeping = true;
			}
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (this.LinearVelocity.Length() != 0)
			this.Rotation = this.LinearVelocity.Angle() + (float)Math.PI / 2;

		if (this.NewVelocity != null)
		{
			if (this.Sleeping)
				this.Sleeping = false;

			this.ApplyCentralImpulse(this.NewVelocity.Value);

			this.NewVelocity = null;
		}
	}
}
