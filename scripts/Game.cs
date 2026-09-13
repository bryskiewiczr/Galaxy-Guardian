using Godot;
using gdtvgalaxyguardian.scripts;

public partial class Game : Node2D {

	private Player _player;
	private PackedScene _projectileScene = GD.Load<PackedScene>(
		"res://scenes/Projectile.tscn");
	
	public override void _Ready() {
		_player = GetNode<Player>("Player");
		_player.ShootProjectile += OnShootProjectile;
	}

	public override void _Process(double delta) {
	}

	public void OnShootProjectile() {
		var projectileInstance = _projectileScene.Instantiate<Projectile>();
		Vector2 projectileStartingPosition = new Vector2(_player.GlobalPosition.X + (float)20.0f, _player.GlobalPosition.Y);
		projectileInstance.GlobalPosition = projectileStartingPosition;
		AddChild(projectileInstance);
	}
}
