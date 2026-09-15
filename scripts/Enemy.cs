using Godot;

public partial class Enemy : Area2D {

    private float _speed = 100.0f;
    private Vector2 _globalPosition;

    public override void _Ready() {
        _globalPosition = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta) {
        _globalPosition.X -= _speed * (float)delta;
        GlobalPosition = _globalPosition;
    }

    public void Destroy() {
        QueueFree();
    }
    
}
