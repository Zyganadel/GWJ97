using Godot;
using System;

public partial class Player : Node3D
{
    [Export] private double maxSpeed;
    [Export] private double acceleration;

    private Vector3 velocity;

    private void movement(double delta) {
        Vector2 movementInput = Input.GetVector("Left", "Right", "Forward", "Backward");
        this.velocity = new Vector3(this.velocity.X + (float)(movementInput.X * acceleration * delta), this.velocity.Y, this.velocity.Z + (float)(movementInput.Y * acceleration * delta));
        if (Vector2(this.velocity.X, this.velocity.Z).Length() > maxSpeed) {

        }
        this.Position = new Vector3(this.Position.X + (float)(movementInput.X * maxSpeed * delta), this.Position.Y, this.Position.Z + (float)(movementInput.Y * maxSpeed * delta));
    }
    public override void _Input(InputEvent @event) {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed) {
            switch (keyEvent.Keycode) {
                case Key.W:
                break;
            }
        }
    }
    public override void _Ready()
    {
        this.velocity = new Vector3(0, 0, 0);
    }
    public override void _Process(double delta)
    {
        movement(delta);
    }
}
