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
		projectileInstance.GlobalPosition = _player.GlobalPosition with { X = _player.GlobalPosition.X + _player.ProjectileSpawnOffset.Position.X };
		AddChild(projectileInstance);
	}
}
