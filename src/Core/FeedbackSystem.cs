using Godot;

namespace GWJ97.Core;

public partial class FeedbackSystem:Node
{
    [Export] PackedScene foeHitScene, foeDeathScene, playerPowerUpScene, playerHitScene;

    // -- overrides --
    public override void _Ready()
    {
        EventSystem.FoeHit += FoeHit;
    }

    void FoeHit(Vector3 pos)
    {
        
    }
    
    // -- helpers --
    void SpawnThingy(PackedScene bla, Vector3 pos = new())
    {
        Node bla2 = bla.Instantiate();
        // todo: hierarchy stuff go here
        if (bla2 is Node3D bla3)
        {
            bla3.Position = pos;
        }
    }
}