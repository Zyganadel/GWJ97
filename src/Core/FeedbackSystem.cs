using Godot;

namespace GWJ97.Core;

public partial class FeedbackSystem:Node
{
    // -- export --
    [Export] PackedScene foeHitScene;
    [Export] PackedScene foeDeathScene;
    [Export] PackedScene playerPowerUpScene;
    [Export] PackedScene playerHitScene;
    [Export] PackedScene fireballScene;
    [Export] PackedScene shockwaveScene;
    [Export] PackedScene beeemScene;

    // -- overrides --
    public override void _Ready()
    {
        EventSystem.PowerChanged += PowerChanged;
        EventSystem.Overpowered += Overpowered;
        EventSystem.FoeDied += FoeDied;
        EventSystem.FoeHit += FoeHit;
        EventSystem.FireballUsed += FireballUsed;
        EventSystem.ShockwaveUsed += ShockwaveUsed;
        EventSystem.BeeemUsed += BeeemUsed;
    }

    void PowerChanged(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(playerPowerUpScene);
    }

    void Overpowered(Vector3 pos, Vector3 rot)
    {
        GD.PrintErr(new System.NotImplementedException());
    }

    void FoeDied(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(foeDeathScene, pos, rot);
    }

    void FoeHit(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(foeHitScene, pos, rot);
    }

    void FireballUsed(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(fireballScene, pos, rot);
    }

    void ShockwaveUsed(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(shockwaveScene, pos);
    }

    void BeeemUsed(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(beeemScene, pos, rot);
    }
    
    // -- helpers --
    void SpawnThingy(PackedScene bla, Vector3 pos = new(), Vector3 rot = new())
    {
        Node bla2 = bla.Instantiate();
        AddChild(bla2);
        if (bla2 is Node3D bla3)
        {
            bla3.Position = pos;
            bla3.Rotation = rot;
        }
    }
}