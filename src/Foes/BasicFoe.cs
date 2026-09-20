using System;
using Godot;
using GWJ97.Core;
using GWJ97.Damage;

namespace GWJ97.Foes;

public partial class BasicFoe:Node3D
{
    // todo: i know this bad practice, i dont give a hoot.
    Hurtbox playerHurtbox;

    // -- export --
    [Export] Hurtbox hurtbox;
    [Export] float speed = 2;
    [Export] int health = 2;
    [Export] PackedScene attackScene;

    // -- prop --


    // -- override --
    public override void _Ready()
    {
        hurtbox.HitTaken += HurtboxOnHitTaken;
    }

    void HurtboxOnHitTaken(int damage)
    {
        health -= damage;
        //EventSystem.FoeHit?.Invoke(GlobalPosition);
        if (health > 0) return;
        //EventSystem.FoeDied?.Invoke(GlobalPosition);
        QueueFree();
    }

    void Attack() {
        var attackInstance = attackScene.Instantiate();
        GetParent().AddChild(attackInstance);
    }

    public override void _PhysicsProcess(double delta)
    {
        //Pretty sure this is a dumb way to give them the player hurtbox but oh well.
        //I am assuming we wanted to use it as an accessible to all enemies var rather than an instance var because that's just more efficient
        //But I do not want to try to figure that out at the moment
        if (!HasNode("../Player/Hurtbox")) {
            return;
        }
        if (playerHurtbox == null)
        {
            playerHurtbox = GetNode<Hurtbox>("../Player/Hurtbox");
        }
        var e = GlobalPosition;
        var p = playerHurtbox.GlobalPosition;
        var vector = (e - p);
        if (vector.Length() < 0.3)
        {
            return;
        }
        var vectorNorm = vector.Normalized();

        GlobalPosition -= vectorNorm * speed * (float)delta;
    }
}
