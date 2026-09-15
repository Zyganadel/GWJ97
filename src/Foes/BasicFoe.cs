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
        if (vector.Length() < speed * delta)
        {
            //This could just be return to keep position because we don't want enemies all in the same position
            GlobalPosition = playerHurtbox.GlobalPosition;
            return;
        }
        var vectorNorm = vector.Normalized();

        GlobalPosition -= vectorNorm * speed * (float)delta;
    }
}
