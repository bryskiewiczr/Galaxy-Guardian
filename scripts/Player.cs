using Godot;

public partial class Player : CharacterBody2D {
    // movement
    [Export] private float _speed = 120.0f;
    private Vector2 _direction = new Vector2(0.0f, 0.0f);
    private AnimationPlayer _animationPlayer;
    private AnimationPlayer _flashAnimationPlayer;
    public Marker2D ProjectileSpawnOffset;
    private CollisionShape2D _playerCollision;
    private Sprite2D _playerSprite;
    
    // screen size
    private float _screenWidth;
    private float _screenHeight;
    
    // timers
    private Timer _invincibilityTimer;
    
    // shooting
    [Signal] public delegate void ShootProjectileEventHandler();

    [Signal] public delegate void PlayerDiedEventHandler();
    
    // flags
    private bool _isAlive = true;
    private bool _isInvincible = false;
    
    public override void _Ready() {
        _screenHeight = GetViewportRect().Size.Y;
        _screenWidth = GetViewportRect().Size.X;
        _animationPlayer = GetNode<AnimationPlayer>("ShipAnimationPlayer");
        _flashAnimationPlayer = GetNode<AnimationPlayer>("FlashAnimationPlayer");
        ProjectileSpawnOffset = GetNode<Marker2D>("ProjectileSpawnOffset");
        _playerCollision = GetNode<CollisionShape2D>("CollisionShape2D");
        _animationPlayer.AnimationFinished += OnAnimationFinished;
        _playerSprite = GetNode<Sprite2D>("Sprite2D");
        _invincibilityTimer = GetNode<Timer>("InvincibilityTimer");
        _invincibilityTimer.Timeout += ToggleInvincibility;
    }

    public void Die() {
        if (_isInvincible) return;
        _animationPlayer.Play("destroy");
        _isAlive = false;
        EmitSignal(SignalName.PlayerDied);
    }

    public void Respawn(Vector2 spawnPos) {
        _isAlive = true;
        _playerSprite.Visible = true;
        GlobalPosition = spawnPos;
        _playerCollision.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        _animationPlayer.Play("default");
        TriggerInvincibility();
    }

    public void TriggerInvincibility() {
        _isInvincible = true;
        _invincibilityTimer.Start();
    }

    public void ToggleInvincibility() {
        _isInvincible = !_isInvincible;
    }
    
    public void Shoot() {
        EmitSignal(SignalName.ShootProjectile);
        _flashAnimationPlayer.Play("default");
    }
    
    public void OnAnimationFinished(StringName animationName) {
        if (animationName == "destroy") {
            _playerSprite.Visible = false;
            _playerCollision.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        }
    }
    
    public void UpdateAnimations() {
        if (_direction.Y < 0) _animationPlayer.Play("up");
        else if (_direction.Y > 0) _animationPlayer.Play("down");
        else _animationPlayer.Play("default");
    }

    public override void _PhysicsProcess(double delta) {
        if (!_isAlive) return;
        
        var horizontalAxis = Input.GetAxis("moveLeft", "moveRight");
        var verticalAxis = Input.GetAxis("moveUp", "moveDown");

        _direction = new Vector2(horizontalAxis, verticalAxis) {
            X = horizontalAxis,
            Y = verticalAxis
        }.Normalized();

        // limit player's position to playable area
        Position = Position.Clamp(new Vector2(0.0f, 0.0f), new Vector2(_screenWidth, _screenHeight));

        // "animate" the sprite
        UpdateAnimations();
        
        // shooting
        if (Input.IsActionJustPressed("shoot")) Shoot();
        
        Velocity = _speed * _direction;
        MoveAndSlide();
    }
}
