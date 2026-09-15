using Godot;
using System;

public partial class Player : Node3D
{
    [Export] private float moveSpeed;
    [Export] private double acceleration;

    private Vector3 velocity;

    private float velocityChangeCalc(float movementInput, float currentVelocity, double delta) {
        float velocity = 0;
        if (movementInput != 0)
        {
            int turning = 1;
            if (Math.Sign(movementInput) != Math.Sign(currentVelocity)) {
                turning = 2;
            }
            velocity = currentVelocity + (float)(movementInput * acceleration * delta * turning);
            if (Math.Abs(velocity) > (Math.Abs(movementInput) * moveSpeed))
            {
                velocity = movementInput * moveSpeed;
            }
        }
        else if (currentVelocity != 0)
        {
            velocity = currentVelocity - (float)(acceleration * delta * Math.Sign(currentVelocity));
            if (Math.Abs(velocity) < (acceleration * delta))
            {
                velocity = 0;
            }
        }
        return velocity;
    }

    private void movement(double delta) {
        float velocityX;
        float velocityZ;
        Vector2 movementInput = Input.GetVector("Left", "Right", "Forward", "Backward");
        velocityX = velocityChangeCalc(movementInput.X, this.velocity.X, delta);
        velocityZ = velocityChangeCalc(movementInput.Y, this.velocity.Z, delta);
        this.velocity = new Vector3(velocityX, this.velocity.Y, velocityZ);
        this.Position = new Vector3(this.Position.X + (float)(velocity.X * delta), this.Position.Y + (float)(velocity.Y * delta), this.Position.Z + (float)(velocity.Z * delta));
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
