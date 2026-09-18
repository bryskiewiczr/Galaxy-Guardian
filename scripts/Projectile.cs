using Godot;

public partial class Projectile : Area2D {
	[Export] private float _projectileSpeed = 500.0f;
	private Vector2 _globalPosition;
	private VisibleOnScreenNotifier2D _notifier2D;
	private AnimatedSprite2D _animatedSprite2D;
	private CollisionShape2D _collision;
	
	public override void _Ready() {
		_globalPosition = GlobalPosition;
		_notifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		_notifier2D.ScreenExited += OnScreenExited;
		_collision = GetNode<CollisionShape2D>("CollisionShape2D");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite2D.AnimationFinished += OnAnimationFinished; // called when projectile hit animation finishes
		AreaEntered += OnEnemyHitByProjectile; // called when enemy is hit by projectile
	}

	public override void _PhysicsProcess(double delta) {
		_globalPosition.X += (_projectileSpeed * (float)delta);
		GlobalPosition = _globalPosition;
	}

	public void OnScreenExited() {
		QueueFree();
	}

	public void OnEnemyHitByProjectile(Area2D area) {
		if (area is Enemy enemy) { 
			_animatedSprite2D.Play("hit_animation");
			_projectileSpeed = 0.0f;
			_collision.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			enemy.Explode();
		}
	}

	public void OnAnimationFinished() {
		if (
			_animatedSprite2D
				.Get(AnimatedSprite2D.PropertyName.Animation)
				.ToString()
				.Equals("hit_animation")
			) {
			QueueFree();
		}
	}
}
