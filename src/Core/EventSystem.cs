using Godot;

namespace GWJ97.Core;

public class EventSystem
{
    public delegate void PositionalThingy(Vector3 pos);

    public static PositionalThingy FoeDied;
    public static PositionalThingy FoeHit;
}