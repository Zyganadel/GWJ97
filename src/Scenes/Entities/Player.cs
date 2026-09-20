using Godot;
using System;
using System.Collections.Generic;
using GWJ97.Core;
using GWJ97.Damage;
using GWJ97.Player;

public partial class Player : Node3D
{
    [Export] float moveSpeed;
    [Export] double acceleration;
    [Export] int health;

    [Export] PackedScene punchScene;
    [Export] PackedScene tentacleSwipeScene;

    public Hurtbox hurtbox;

    private Vector3 velocity;

    private Vector2 direction = new Vector2(0, 0);

    private bool attacking = false;

    private AnimatedSprite3D sprite;
    private AnimatedSprite3D tentacleSprite;
    private PowerComponent powerComponent;

    Dictionary<string, Vector3> tentaclePositions = new Dictionary<string, Vector3> {
        {"Forward", new Vector3((float)-0.02, (float)0.18, (float)0)},
        {"Backward", new Vector3((float)0.0003, (float)0.238, (float)0.002)},
        {"Right", new Vector3((float)-0.17, (float)0.238, (float)0.002)},
        {"Left", new Vector3((float)0.178, (float)0.221, (float)-0.085)}
    };

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

    private void updateCurrentAnimation(string appendedString)
    {
        string currentAnimation = sprite.Animation;
        if (currentAnimation.Contains(appendedString))
        {
            return;
        }
        string[] currentAnimationSplit = currentAnimation.Split(" ");
        String animation = currentAnimationSplit[0] + " " + appendedString;
        sprite.Play(animation);
    }
    void updateTentacleDir(string playerFacing)
    {
        tentacleSprite.Position = tentaclePositions[playerFacing];
        if (playerFacing == "Backward" || playerFacing == "Left")
        {
            tentacleSprite.FlipH = true;
        }
        else
        {
            tentacleSprite.FlipH = false;
        }
        string animationDirection = playerFacing;
        if (animationDirection == "Left" || animationDirection == "Right")
        {
            animationDirection = "Side";
        }
        tentacleSprite.Animation = animationDirection + " Swipe";
    }

    private void onPowerChanged() {
        if (powerComponent.PowerLevel > 1) {
            tentacleSprite.Visible = true;
            string currentAnimation = sprite.Animation;
        } else {
            tentacleSprite.Visible = false;
        }
    }


    private void movement(double delta) {
        float velocityX;
        float velocityZ;
        Vector2 movementInput = Input.GetVector("Left", "Right", "Forward", "Backward");
        if (movementInput != Vector2.Zero) {
            direction = movementInput;
        }
        velocityX = velocityChangeCalc(movementInput.X, this.velocity.X, delta);
        velocityZ = velocityChangeCalc(movementInput.Y, this.velocity.Z, delta);
        if (velocityX == 0 && velocityZ == 0 && !attacking)
        {
            updateCurrentAnimation("Idle");
        }
        this.velocity = new Vector3(velocityX, this.velocity.Y, velocityZ);
        this.Position = new Vector3(this.Position.X + (float)(velocity.X * delta), this.Position.Y + (float)(velocity.Y * delta), this.Position.Z + (float)(velocity.Z * delta));
    }
    public override void _Ready()
    {
        this.velocity = new Vector3(0, 0, 0);
        hurtbox = GetNode<Hurtbox>("Hurtbox");
        hurtbox.HitTaken += hurtboxOnHitTaken;
        sprite = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
        tentacleSprite = GetNode<AnimatedSprite3D>("TentacleSprite");
        powerComponent = GetNode<PowerComponent>("PowerComponent");
        PowerComponent.PowerChanged += onPowerChanged;

    }
    public override void _Process(double delta)
    {
        movement(delta);
    }
    public override void _Input(InputEvent @event) {
        if (@event.IsActionPressed("Forward"))
        {
            sprite.Play("Forward Walk");
            sprite.FlipH = false;
            updateTentacleDir("Forward");
        }
        if (@event.IsActionPressed("Backward"))
        {
            sprite.Play("Backward Walk");
            sprite.FlipH = false;
            updateTentacleDir("Backward");
        }
        if (@event.IsActionPressed("Right"))
        {
            sprite.FlipH = false;
            sprite.Play("Side Walk");
            updateTentacleDir("Right");
        }
        if (@event.IsActionPressed("Left"))
        {
            sprite.FlipH = true;
            sprite.Play("Side Walk");
            updateTentacleDir("Left");
        }
        if (@event.IsActionPressed("Bump Power"))
        {
            powerComponent.BumpPower();
        }
        if (@event.IsActionReleased("Forward") || @event.IsActionReleased("Backward") || @event.IsActionReleased("Right") || @event.IsActionReleased("Left"))
        {
            if (Input.IsActionPressed("Forward"))
            {
                sprite.Play("Forward Walk");
                sprite.FlipH = false;	// Called when the node enters the scene tree for the first time.
                updateTentacleDir("Forward");
            }
            if (Input.IsActionPressed("Backward"))
            {
                sprite.Play("Backward Walk");
                sprite.FlipH = false;
                updateTentacleDir("Backward");
            }
            if (Input.IsActionPressed("Right"))
            {
                sprite.FlipH = false;
                sprite.Play("Side Walk");
                updateTentacleDir("Right");
            }
            if (Input.IsActionPressed("Left"))
            {
                sprite.FlipH = true;
                sprite.Play("Side Walk");
                updateTentacleDir("Left");
            }
        }
        if (!GetNode<Timer>("AttackCooldown").IsStopped() || attacking)
        {
            return;
        }
        if (@event.IsActionPressed("Ability1")) {
            attacking = true;
            if (powerComponent.PowerLevel < 2) {
                Node3D punchInstance = (Node3D)punchScene.Instantiate();
                punchInstance.Position = new Vector3((float)(0.35 * direction.X), 0, (float)(0.35 * direction.Y));
                AddChild(punchInstance);
                updateCurrentAnimation("Punch");
                this.velocity = Vector3.Zero;
                this.acceleration = 0.2 * this.acceleration;
            } else {
                Node3D tentacleSwipeInstance = (Node3D)tentacleSwipeScene.Instantiate();
                tentacleSwipeInstance.Position = new Vector3((float)(0.35) * direction.X, 0, (float)(0.35 * direction.Y));
                tentacleSwipeInstance.Rotation = new Vector3(0, -direction.Angle(), 0);
                AddChild(tentacleSwipeInstance);
                tentacleSprite.Play();
                this.velocity = Vector3.Zero;
                this.acceleration = 0.2 * this.acceleration;
            }

        }
    }

    void hurtboxOnHitTaken(int damage)
    {
        GD.Print("Player Hit");
    }

    void onAttackTimout() {
        GetNode<Timer>("AttackCooldown").Start(0.5);
        attacking = false;
        this.acceleration = 5 * this.acceleration;
        if (this.velocity == Vector3.Zero) {
            updateCurrentAnimation("Idle");
        } else {
            updateCurrentAnimation("Walk");
        }
    }

}
