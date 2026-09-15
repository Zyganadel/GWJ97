using Godot;
using System;
using GWJ97.Core;
using GWJ97.Damage;

public partial class Player : Node3D
{
    [Export] float moveSpeed;
    [Export] double acceleration;
    [Export] int health;

    public Hurtbox hurtbox;

    private Vector3 velocity;

//Runs twice per frame for the X and Z axis velocities. calculates velocity based on previous frame and current buttons pressed.
    private float velocityChangeCalc(float movementInput, float currentVelocity, double delta)
    {
        float velocity = 0;
        if (movementInput != 0)
        {
            int turning = 1;
            if (Math.Sign(movementInput) != Math.Sign(currentVelocity))
            {
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
        if (velocityX == 0 && velocityZ == 0)
        {
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").Stop();
        }
        this.velocity = new Vector3(velocityX, this.velocity.Y, velocityZ);
        this.Position = new Vector3(this.Position.X + (float)(velocity.X * delta), this.Position.Y + (float)(velocity.Y * delta), this.Position.Z + (float)(velocity.Z * delta));
    }
    public override void _Ready()
    {
        this.velocity = new Vector3(0, 0, 0);
        hurtbox.HitTaken += hurtboxOnHitTaken;
        hurtbox = GetNode<Hurtbox>("Hurtbox");
    }
    public override void _Process(double delta)
    {
        movement(delta);
    }
    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("Forward"))
        {
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Forward");
        }
        if (@event.IsActionPressed("Backward"))
        {
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Backward");
        }
        if (@event.IsActionPressed("Right"))
        {
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").FlipH = false;
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Side");
        }
        if (@event.IsActionPressed("Left"))
        {
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").FlipH = true;
            GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Side");
        }
        if (@event.IsReleased()) {
            if (Input.IsActionPressed("Forward"))
            {
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Forward");
            }
            if (Input.IsActionPressed("Backward"))
            {
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Backward");
            }
            if (Input.IsActionPressed("Right"))
            {
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").FlipH = false;
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Side");
            }
            if (Input.IsActionPressed("Left")) {
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").FlipH = true;
                GetNode<AnimatedSprite3D>("AnimatedSprite3D").Play("Side");
            }
        }
    }

    void hurtboxOnHitTaken() {
        GD.Print("Player Hit");
    }
}
