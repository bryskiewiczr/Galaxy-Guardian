using Godot;
using System;

namespace gdtvgalaxyguardian.scripts;

public partial class Player : CharacterBody2D {

    private float _speed = 100.0f;
    private Vector2 _direction = new(1.0f, 0.0f);

    public override void _Ready() { }
    
    public override void _PhysicsProcess(double delta) {
        Velocity = _speed * _direction;
        MoveAndSlide();
    }
}
