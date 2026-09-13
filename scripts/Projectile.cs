using Godot;

public partial class Projectile : Area2D {
	[Export] private float _projectileSpeed = 500.0f;
	private Vector2 _globalPosition;
	
	public override void _Ready() {
		_globalPosition = GlobalPosition;
	}

	public override void _PhysicsProcess(double delta) {
		_globalPosition.X += (_projectileSpeed * (float)delta);
		GlobalPosition = _globalPosition;
	}
}
