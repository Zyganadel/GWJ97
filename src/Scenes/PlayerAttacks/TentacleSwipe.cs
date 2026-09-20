using Godot;
using System;
using GWJ97.Damage;

public partial class TentacleSwipe : Hitbox
{
	public override void _Ready()
    {
        damage = 2;
	}

}
