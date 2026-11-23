using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using TheValkyrie.TheValkyrieCode.Relics.NestBirdpyps;

namespace TheValkyrie.TheValkyrieCode.Pets;

public sealed class BlueByrdpipPet : MonsterModel
{
    public override int MinInitialHp => 9999;

    public override int MaxInitialHp => 9999;

    public override bool IsInteractable => false;
    
    protected override string VisualsPath => SceneHelper.GetScenePath("creature_visuals/byrdpip"); //placeholder

    public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
    {
        string skinName = !IsMutable ? BlueByrdpip.SkinOptions[0] : Creature.PetOwner?.GetRelic<BlueByrdpip>()?.Skin ?? BlueByrdpip.SkinOptions[0];
        MegaSkeletonDataResource data = skeleton.GetData();
        skeleton.SetSkin(data.FindSkin(skinName));
        skeleton.SetSlotsToSetupPose();
        
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        MoveState initialState = new MoveState("NOTHING_MOVE",  _ => Task.CompletedTask);
        initialState.FollowUpState = initialState;
        return new MonsterMoveStateMachine([initialState], initialState);
    }
}