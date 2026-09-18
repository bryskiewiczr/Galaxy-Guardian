using Godot;

public partial class Game : Node2D {

	private Player _player;
	private PackedScene _projectileScene = GD.Load<PackedScene>(
		"res://scenes/Projectile.tscn");

	private EnemySpawner _enemySpawner;
	private Area2D _enemyDespawner;
	private Marker2D _playerSpawner;
	private Hud _hud;

	private int _lives = 3;
	private int _score;
	private Timer _respawnTimer;
	
	private AudioStreamPlayer2D _projectileShotSound;
	private AudioStreamPlayer2D _explosionSound;
	
	private PackedScene _gameOverScene = GD.Load<PackedScene>("res://scenes/GameOver.tscn");
	
	public override void _Ready() {
		_player = GetNode<Player>("Player");
		_player.ShootProjectile += OnShootProjectile;
		_player.PlayerDied += OnPlayerDied;
		_enemySpawner = GetNode<EnemySpawner>("EnemySpawner");
		_enemySpawner.EnemySpawned += OnEnemySpawned;
		_enemyDespawner = GetNode<Area2D>("EnemyDespawner");
		_enemyDespawner.AreaEntered += OnEnemyDespawnerAreaEntered;
		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_respawnTimer.Timeout += OnRespawnTimerTimeout;
		_playerSpawner = GetNode<Marker2D>("PlayerSpawner");
		_hud = GetNode<Hud>("CanvasLayer/HUD");
		_hud.SetLivesLabel(_lives);
		_hud.SetScoreLabel(_score);
		_projectileShotSound = GetNode<AudioStreamPlayer2D>("ProjectileShotSound");
		_explosionSound = GetNode<AudioStreamPlayer2D>("ExplosionSound");
	}
	
	private async void OnPlayerDied() {
		_explosionSound.Play();
		_lives -= 1;
		_hud.SetLivesLabel(_lives);
		if (_lives <= 0) {
			await ToSignal(GetTree().CreateTimer(1.5f), Timer.SignalName.Timeout);
			var gameOverScreen = _gameOverScene.Instantiate<GameOver>();
			_hud.AddChild(gameOverScreen);
			return;
		}
		_respawnTimer.Start(); // start respawn timer
	}

	private void OnRespawnTimerTimeout() {
		GD.Print("Respawn Timer Timed Out");
		_player.Respawn(_playerSpawner.Position);
	}
	
	private void OnShootProjectile() {
		var projectileInstance = _projectileScene.Instantiate<Projectile>();
		projectileInstance.GlobalPosition = _player.GlobalPosition with { X = _player.GlobalPosition.X + _player.ProjectileSpawnOffset.Position.X };
		AddChild(projectileInstance);
		_projectileShotSound.Play();
	}

	private void OnEnemyDestroyed() {
		_score += 100;
		_explosionSound.Play();
		_hud.SetScoreLabel(_score);
	}
	
	private void OnEnemySpawned(Enemy enemy) {
		enemy.Destroyed += OnEnemyDestroyed;
		AddChild(enemy);
	}

	private void OnEnemyDespawnerAreaEntered(Area2D area) {
		if (area is Enemy) area.QueueFree();
	}
}
