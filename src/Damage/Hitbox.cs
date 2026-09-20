using Godot;

namespace GWJ97.Damage;

/// <summary>
/// Represents a thing that hurts.
/// </summary>
public partial class Hitbox:Area3D
{
    /// <summary>
    /// Used to prevent friendly fire. Assumes player will be 0, foes will be 1.
    /// See also <see cref="Hurtbox.team"/>
    /// </summary>
    [Export] public int team = 1;

    protected int damage = 0;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    void OnAreaEntered(Area3D area)
    {
        if (area is not Hurtbox target) return;
        if (target.team == team) return;
        target.TakeHit(damage);
    }
}
