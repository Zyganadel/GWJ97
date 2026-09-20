using System;
using Godot;

namespace GWJ97.Player;

public partial class PowerComponent : Node
{
    // -- Events --
    public static event Action Overpowered;
    public static event Action PowerChanged;

    [Signal] public delegate void powerChangedEventHandler(int powerLevel);

    // -- Export --
    /// <summary>
    /// The maximum power the player can hold before they die.
    /// </summary>
    [Export] int maxPower = 5;

    /// <summary>
    /// The number of physics ticks it should take to lose a power level.
    /// </summary>
    [Export] int decayRate = 600;

    // -- Prop --
    /// <summary>
    /// The player's power level
    /// </summary>
    public int PowerLevel
    {
        get => powerLevel;
        private set
        {
            powerLevel = int.Max(value, 0);
            PowerChanged?.Invoke();
            EmitSignal(SignalName.powerChanged, powerLevel);     //This is absolutely redundant with the above line i just dont know how events work and i wrote the reciever in gdscript
        }
    }

    // -- Backers --
    int powerLevel = 0;

    // -- Field --
    int ticks;

    // -- Override --
    public override void _Ready() {
        powerLevel = 0;
    }
    public override void _PhysicsProcess(double delta)
    {
        CheckPower();
    }

    // -- Methods --

    /// <summary>
    /// Raises the power level by 1.
    /// </summary>
    public void BumpPower()
    {
        PowerLevel++;
        ticks = 0;
        if (powerLevel > maxPower) Overpowered?.Invoke();
    }

    // Check how long its been since we updated power level. if greater than decay rate, drop power by 1.
    void CheckPower()
    {
        ticks++;
        if (ticks < decayRate) return;
        ticks = 0;
        PowerLevel--;
    }
}
