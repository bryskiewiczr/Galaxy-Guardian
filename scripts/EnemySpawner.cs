using Godot;
using System;

public partial class EnemySpawner : Node2D {

    [Signal]
    public delegate void EnemySpawnedEventHandler(Enemy enemyInstance);
    
    private Timer _timer;
    private Node2D _spawnPositions;
    private Random _random;
    private PackedScene _enemyScene = GD.Load<PackedScene>("res://scenes/Enemy.tscn");

    public override void _Ready() {
        _timer = GetNode<Timer>("SpawnTimer");
        _spawnPositions = GetNode<Node2D>("SpawnPositions");
        _random = new Random();
        
        _timer.Timeout += OnTimeoutSpawnEnemy;
    }
    
    private void OnTimeoutSpawnEnemy() {
        var spawner = PickSpawner();
        var spawnerPosition = spawner.GlobalPosition;
        SpawnEnemy(position: spawnerPosition);
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
