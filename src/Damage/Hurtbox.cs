using System;
using Godot;

namespace GWJ97.Damage;

/// <summary>
/// Represents a thing that can be hurt.
/// </summary>
public partial class Hurtbox:Area3D
{
    public event Action<int> HitTaken;

    /// <summary>
    /// Used to prevent friendly fire. Assumes player will be 0, foes will be 1.
    /// See also <see cref="Hitbox.team"/>
    /// </summary>
    [Export] public int team=1;

    /// <summary>
    /// Causes this hurtbox to take damage.
    /// </summary>
    public void TakeHit(int damage)
    {
        HitTaken?.Invoke(damage);
    }
}
