using System;
using Godot;
using GWJ97.Core;
using GWJ97.Damage;

namespace GWJ97.Foes;

public partial class BasicFoe:Node3D
{
    // todo: i know this bad practice, i dont give a hoot.
    public static Hurtbox playerHurtbox;
    
    // -- export --
    [Export] Hurtbox hurtbox;
    [Export] float speed = 1;
    [Export] int health = 2;
    
    // -- prop --
    
    
    // -- override --
    public override void _Ready()
    {
        hurtbox.HitTaken += HurtboxOnHitTaken;
    }

    void HurtboxOnHitTaken()
    {
        health--;
        EventSystem.FoeHit?.Invoke(GlobalPosition);
        if (health > 0) return;
        EventSystem.FoeDied?.Invoke(GlobalPosition);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        var e = GlobalPosition;
        var p = playerHurtbox.GlobalPosition;
        var vector = (e - p).Normalized();
        
        GlobalPosition -= vector * speed;
    }
}