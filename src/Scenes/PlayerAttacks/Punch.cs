using Godot;
using System;
using GWJ97.Damage;

public partial class Punch : Hitbox
{
    public override void _Ready() {
        damage = 1;
    }
}
