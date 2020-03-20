using Godot;
using System;

public class Player : Area2D
{
	[Signal]
	public delegate void Hit();

	[Export]
	public int Speed = 400;

	[Export]
	public int ClickRadius = 32;

	private Vector2 ScreenSize;
	private Boolean Dragging = false;

	private AnimatedSprite GetSprite() => base.GetNode<AnimatedSprite>("AnimatedSprite");

	public override void _Ready()
	{
		this.ScreenSize = base.GetViewport().Size;
	}

	public override void _Input(InputEvent inputEvent)
	{
		var sprite = this.GetSprite();

		if (inputEvent is InputEventMouseButton mouseEvent && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
		{
			if ((mouseEvent.Position - sprite.Position).Length() < this.ClickRadius)
			{
				// Start dragging if the click is on the sprite.
				if (!this.Dragging && mouseEvent.Pressed)
					this.Dragging = true;
			}

			// Stop dragging if the button is released.
			if (this.Dragging && !mouseEvent.Pressed)
			{
				this.Dragging = false;
			}
		}
		else if (inputEvent is InputEventMouseMotion motionEvent && this.Dragging)
		{
			// While dragging, move the sprite with the mouse.
			sprite.Position = motionEvent.Position;
		}
	}

	public override void _Process(float delta)
	{
		var velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
			velocity.x += 1;

		if (Input.IsActionPressed("ui_left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("ui_down"))
			velocity.y += 1;

		if (Input.IsActionPressed("ui_up"))
			velocity.y -= 1;

		var animatedSprite = this.GetSprite();

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * this.Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}

		base.Position += velocity * delta;
		base.Position = new Vector2(
			x: Mathf.Clamp(base.Position.x, 0, this.ScreenSize.x),
			y: Mathf.Clamp(base.Position.y, 0, this.ScreenSize.y)
		);
	}

	public void OnPlayerBodyEntered(PhysicsBody2D body)
	{
		base.Hide();
		base.EmitSignal("Hit");
		base.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabeld", true);
	}

	/// <summary>Reset the player when starting a new game.</summary>
	public void Start(Vector2 pos)
	{
		base.Position = pos;
		base.Show();
		base.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
