using Godot;

public partial class Enemy : Area2D {

    private float _speed = 100.0f;
    private Vector2 _globalPosition;
    private AnimationPlayer _animationPlayer;
    private CollisionPolygon2D _collision;

    [Signal]
    public delegate void DestroyedEventHandler();

    public override void _Ready() {
        _globalPosition = GlobalPosition;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _collision = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        BodyEntered += KillPlayer;
    }

    public override void _PhysicsProcess(double delta) {
        _globalPosition.X -= _speed * (float)delta;
        GlobalPosition = _globalPosition;
    }

    public void Explode() {
        _speed = 0.0f;
        _animationPlayer.Play("destroy");
        _collision.SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);
        EmitSignal(SignalName.Destroyed);
    }

    public void Destroy() {
        QueueFree();
    }

    private void KillPlayer(Node2D body) {
        if (body is Player player) {
            player.Die();
            Explode();
        }
    }
}
