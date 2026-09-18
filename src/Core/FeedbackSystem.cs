using Godot;

namespace GWJ97.Core;

public partial class FeedbackSystem:Node
{
    // -- export --
    [Export] PackedScene foeHitScene, foeDeathScene, playerPowerUpScene, playerHitScene;

    // -- overrides --
    public override void _Ready()
    {
        EventSystem.FoeDied += FoeDied;
        EventSystem.FoeHit += FoeHit;
    }

    void FoeDied(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(foeDeathScene, pos, rot);
    }

    void FoeHit(Vector3 pos, Vector3 rot)
    {
        SpawnThingy(foeHitScene, pos, rot);
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