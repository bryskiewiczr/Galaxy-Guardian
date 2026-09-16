using Godot;
using System;

public partial class EnemySpawner : Node2D {

    [Signal]
    public delegate void EnemySpawnedEventHandler(Enemy enemyInstance);
    
    private Timer _spawnTimer;
    private Timer _difficultyTimer;
    private Node2D _spawnPositions;
    private Random _random;
    private PackedScene _enemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");
    
    private float _difficultyStep = 0.25f;
    private float _maxDifficultyTimer = 0.25f;

    public override void _Ready() {
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _difficultyTimer = GetNode<Timer>("DifficultyTimer");
        _spawnPositions = GetNode<Node2D>("SpawnPositions");
        _random = new Random();
        
        _spawnTimer.Timeout += OnTimeoutSpawnEnemy;
        _difficultyTimer.Timeout += OnTimeoutIncreaseDifficulty;
    }
    
    private void OnTimeoutSpawnEnemy() {
        var spawner = PickSpawner();
        var spawnerPosition = spawner.GlobalPosition;
        SpawnEnemy(position: spawnerPosition);
    }

    private void OnTimeoutIncreaseDifficulty() {
        if (_spawnTimer.WaitTime <= _maxDifficultyTimer) return;
        _spawnTimer.WaitTime -= _difficultyStep;
    }

    private Marker2D PickSpawner() {
        var spawnerId = _random.Next(0, _spawnPositions.GetChildCount());
        var spawner = (Marker2D)_spawnPositions.GetChild(spawnerId);
        return spawner;
    }
    
    private void SpawnEnemy(Vector2 position) {
        var enemyInstance = _enemyScene.Instantiate<Enemy>();
        enemyInstance.GlobalPosition = position;
        EmitSignal(SignalName.EnemySpawned, enemyInstance);
    }
}
