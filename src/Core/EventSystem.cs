using Godot;

namespace GWJ97.Core;

public class EventSystem
{
    public delegate void PositionalThingy(Vector3 pos, Vector3 rot = new());

    // -- Damage stuff --
    public static PositionalThingy PowerChanged;
    public static PositionalThingy Overpowered;
    public static PositionalThingy FoeDied;
    public static PositionalThingy FoeHit;
    
    // -- Ability stuff --
    public static PositionalThingy FireballUsed;
}