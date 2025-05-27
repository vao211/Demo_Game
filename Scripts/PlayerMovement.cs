using Godot;
namespace DemoGame.Scripts;
public partial class PlayerMovement : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	private AnimatedSprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (_sprite == null)
		{
			GD.PrintErr("AnimatedSprite2D node not found!");
			return;
		}
		if (_sprite.SpriteFrames == null)
		{
			GD.PrintErr("SpriteFrames not assigned!");
			return;
		}

		// Kiểm tra sự tồn tại của các animation
		string[] requiredAnimations = { "walk_up", "walk_down", "walk_right", "walk_left", 
									   "idle_up", "idle_down", "idle_right", "idle_left" };
		foreach (string anim in requiredAnimations)
		{
			if (!_sprite.SpriteFrames.HasAnimation(anim))
			{
				GD.PrintErr($"Animation '{anim}' not found in SpriteFrames!");
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection * Speed;
		UpdateAnimation(inputDirection);
		MoveAndSlide();
	}

	private void UpdateAnimation(Vector2 direction)
	{
		if (_sprite == null || _sprite.SpriteFrames == null) return;

		if (direction == Vector2.Zero)
		{
			// Chọn animation idle dựa trên hướng trước đó, mặc định là idle_down
			string idleAnim = "idle_down";
			if (_sprite.Animation == "walk_up" || _sprite.Animation == "idle_up")
			{
				idleAnim = "idle_up";
			}
			else if (_sprite.Animation == "walk_down" || _sprite.Animation == "idle_down")
			{
				idleAnim = "idle_down";
			}
			else if (_sprite.Animation == "walk_right" || _sprite.Animation == "idle_right")
			{
				idleAnim = "idle_right";
			}
			else if (_sprite.Animation == "walk_left" || _sprite.Animation == "idle_left")
			{
				idleAnim = "idle_left";
			}

			if (_sprite.SpriteFrames.HasAnimation(idleAnim))
			{
				_sprite.Play(idleAnim);
			}
			else
			{
				GD.PrintErr($"Animation '{idleAnim}' not found in SpriteFrames!");
			}
		}
		else
		{
			// Chọn animation walk dựa trên hướng di chuyển
			string walkAnim = "walk_down"; // Mặc định
			if (Mathf.Abs(direction.Y) > Mathf.Abs(direction.X))
			{
				// Ưu tiên hướng dọc
				walkAnim = direction.Y < 0 ? "walk_up" : "walk_down";
			}
			else
			{
				// Ưu tiên hướng ngang
				walkAnim = direction.X < 0 ? "walk_left" : "walk_right";
			}

			if (_sprite.SpriteFrames.HasAnimation(walkAnim))
			{
				_sprite.Play(walkAnim);
			}
			else
			{
				GD.PrintErr($"Animation '{walkAnim}' not found in SpriteFrames!");
			}
		}
	}
}
