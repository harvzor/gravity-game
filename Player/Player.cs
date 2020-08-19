using Godot;
using System;
using System.Threading.Tasks;

public class Player : RigidBody2D
{
	[Signal]
	public delegate void FuelUsed(int newFuelValue);
	[Signal]
	public delegate void TimeFuelUsed(int newTimeFuelValue);

	[Export]
	public int Speed = 2;

	[Export]
	public int ClickRadius = 64;

	[Export]
	public float ZoomStep = 0.5f;

	public Boolean CanTeleport = true;

	private readonly Timer TimeFuelTimer = new Timer();
	private Vector2 InitialPosition;
	private float InitialRotation;
	private Vector2? NewVelocity;
	private Boolean ShouldReset;
	private Boolean ShouldSlowDown;
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

			this.MaybeSlowTime();
		}
	}

	private Vector2? DragCurrentPosition;
	private Vector2? DragEndPosition;

	private Int32 _Fuel;
	private Int32 Fuel
	{
		get => this._Fuel;
		set
		{
			this._Fuel = value;

			this.EmitSignal(nameof(FuelUsed), this.Fuel);
		}
	}
	private Int32 _TimeFuel;
	private Int32 TimeFuel
	{
		get => this._TimeFuel;
		set
		{
			this._TimeFuel = value;

			this.EmitSignal(nameof(TimeFuelUsed), this.TimeFuel);
		}
	}

	private Global Global => base.GetNode<Global>("/root/Global");
	private CanvasModulate CanvasModulate => base.GetNode<CanvasModulate>("../CanvasModulate");
	private Particles2D Death => base.GetNode<Particles2D>("Death");

	private Node2D Sprite => base.GetNode<Node2D>("Sprite");
	private CollisionPolygon2D CollisionShape => base.GetNode<CollisionPolygon2D>("CollisionShape2D");
	public LinePath LinePath => base.GetNode<LinePath>("LinePath");
	private Line Line => base.GetNode<Line>("Line");
	private Node2D Light => base.GetNode<Node2D>("Light");
	public Camera2D Camera => base.GetNode<Camera2D>("Camera");

	private void SetupTimeFuelTimer()
	{
		base.AddChild(this.TimeFuelTimer);

		this.TimeFuelTimer.WaitTime = 0.01f;
		this.TimeFuelTimer.OneShot = false;

		this.TimeFuelTimer.Connect("timeout", this, nameof(this.CalculateTimeFuel));

		this.TimeFuelTimer.Start();
	}

	private void MaybeSlowTime()
	{
		if (this.Dragging && this.TimeFuel > 0)
		{
			this.Global.TimeScale = 0.1f;

			if (this.CanvasModulate != null)
			{
				var color = this.CanvasModulate.Color;
				color.a = 2;

				this.CanvasModulate.Color = color;
			}
		}
		else
		{
			this.Global.TimeScale = 1f;

			if (this.CanvasModulate != null)
			{
				var color = this.CanvasModulate.Color;
				color.a = 1;

				this.CanvasModulate.Color = color;
			}
		}
	}

	private void CalculateTimeFuel()
	{
		if (this.Dragging)
		{
			if (this.Sleeping == false)
				this.TimeFuel--;

			this.MaybeSlowTime();
		}
	}

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

		this.Fuel = this.Fuel - fuelUsage;

		this.NewVelocity = velocity;
	}

	private Int32 CalculateFuelUsage(Vector2 velocity)
	{
		return 0;
		// return (int)Math.Ceiling(velocity.Length() / 100);
	}

	public override void _Ready()
	{
		this.InitialPosition = base.Position;
		this.InitialRotation = base.Rotation;

		this.ScreenSize = base.GetViewport().Size;

		this.Start();

		this.ShouldSleep = true;

		this.Fuel = 100;
		this.TimeFuel = 100;

		if (this.Global.Zoom != null)
			this.Camera.Zoom = this.Global.Zoom.Value;

		this.SetupTimeFuelTimer();
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
	}

	public void Zoom(float delta)
	{
		var newZoom = new Vector2(
			x: this.Camera.Zoom.x * Math.Abs(this.ZoomStep + delta),
			y: this.Camera.Zoom.y * Math.Abs(this.ZoomStep + delta)
		);

		if (newZoom < this.MinZoom)
			newZoom = this.MinZoom;

		this.NewZoom = newZoom;

		this.Global.Zoom = this.NewZoom.Value;
	}

	public async Task Crash()
	{
		this.CollisionShape.SetDeferred("disabled", true);

		this.Sprite.Hide();
		this.Light.Hide();

		this.ShouldSleep = true;

		this.Global.CrashSound.Play();

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

	public void OnGoal()
	{
		this.ShouldSleep = true;

		this.Global.Coin.Play();
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
			this.Camera.Zoom = this.Camera.Zoom.MoveToward(this.NewZoom.Value, Math.Abs(this.Camera.Zoom.x - this.NewZoom.Value.x) * delta * 10);
		}
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (this.ShouldSlowDown)
		{
			state.LinearVelocity = state.LinearVelocity.MoveToward(new Vector2(x: 0, y: 0), GetProcessDeltaTime() * 2000);
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
		if (this.LinearVelocity.Length() != 0 && this.ShouldSlowDown == false)
			this.Rotation = this.LinearVelocity.Angle() + (float)Math.PI / 2;

		if (this.NewVelocity != null)
		{
			if (this.Sleeping)
			{
				this.Sleeping = false;
			}

			this.ApplyCentralImpulse(this.NewVelocity.Value);

			this.NewVelocity = null;
		}
	}
}
