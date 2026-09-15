using Godot;

public partial class Projectile : Area2D {
	[Export] private float _projectileSpeed = 500.0f;
	private Vector2 _globalPosition;
	private VisibleOnScreenNotifier2D _notifier2D;
	
	public override void _Ready() {
		_globalPosition = GlobalPosition;
		_notifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		_notifier2D.ScreenExited += OnScreenExited;
	}

	public override void _PhysicsProcess(double delta) {
		_globalPosition.X += (_projectileSpeed * (float)delta);
		GlobalPosition = _globalPosition;
	}

	public void OnScreenExited() {
		QueueFree();
	}
}
