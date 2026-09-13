using Godot;
using System;

namespace gdtvgalaxyguardian.scripts;

public partial class Player : CharacterBody2D {
    // movement
    [Export] private float _speed = 120.0f;
    private Vector2 _direction = new Vector2(0.0f, 0.0f);
    private AnimationPlayer _animationPlayer;

    // screen size
    private float _screenWidth;
    private float _screenHeight;
    
    // shooting
    [Signal] public delegate void ShootProjectileEventHandler();

    public void Shoot() {
        EmitSignal(SignalName.ShootProjectile);
    }
    
    public override void _Ready() {
        _screenHeight = GetViewportRect().Size.Y;
        _screenWidth = GetViewportRect().Size.X;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta) {
        var horizontalAxis = Input.GetAxis("moveLeft", "moveRight");
        var verticalAxis = Input.GetAxis("moveUp", "moveDown");

        _direction = new Vector2(horizontalAxis, verticalAxis) {
            X = horizontalAxis,
            Y = verticalAxis
        }.Normalized();

        // limit player's position to playable area
        Position = Position.Clamp(new Vector2(0.0f, 0.0f), new Vector2(_screenWidth, _screenHeight));

        // "animate" the sprite
        if (_direction.Y < 0) _animationPlayer.Play("up");
        else if (_direction.Y > 0) _animationPlayer.Play("down");
        else _animationPlayer.Play("default");
        
        // shooting
        if (Input.IsActionJustPressed("shoot")) Shoot();
        
        Velocity = _speed * _direction;
        MoveAndSlide();
    }
}