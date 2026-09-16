using Godot;
using gdtvgalaxyguardian.scripts;

public partial class Game : Node2D {

	private Player _player;
	private PackedScene _projectileScene = GD.Load<PackedScene>(
		"res://scenes/Projectile.tscn");

	private EnemySpawner _enemySpawner;
	
	public override void _Ready() {
		_player = GetNode<Player>("Player");
		_player.ShootProjectile += OnShootProjectile;
		_enemySpawner = GetNode<EnemySpawner>("EnemySpawner");
		_enemySpawner.EnemySpawned += OnEnemySpawned;
	}

	public override void _Process(double delta) {
	}

	private void OnShootProjectile() {
		var projectileInstance = _projectileScene.Instantiate<Projectile>();
		projectileInstance.GlobalPosition = _player.GlobalPosition with { X = _player.GlobalPosition.X + _player.ProjectileSpawnOffset.Position.X };
		AddChild(projectileInstance);
	}

	private void OnEnemySpawned(Enemy enemy) {
		AddChild(enemy);
	}
}
