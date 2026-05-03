using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

#nullable enable
namespace TheValkyrie.TheValkyrieCode.Pets;

public sealed class YellowByrdpipPet : MonsterModel
{
    public override int MinInitialHp => 9999;

    public override int MaxInitialHp => 9999;

    public override bool IsHealthBarVisible => false;
    
    protected override string VisualsPath => SceneHelper.GetScenePath("creature_visuals/byrdpip"); //placeholder

    public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
    {
        string skinName = !this.IsMutable ? Relics.NestBirdpyps.YellowByrdpip.SkinOptions[0] : this.Creature.PetOwner?.GetRelic<Relics.NestBirdpyps.YellowByrdpip>()?.Skin ?? Relics.NestBirdpyps.YellowByrdpip.SkinOptions[0];
        MegaSkeletonDataResource data = skeleton.GetData();
        skeleton.SetSkin(data.FindSkin(skinName));
        skeleton.SetSlotsToSetupPose();
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        MoveState initialState = new MoveState("NOTHING_MOVE",  _ => Task.CompletedTask, Array.Empty<AbstractIntent>());
        initialState.FollowUpState = initialState;
        return new MonsterMoveStateMachine([initialState], initialState);
    }
}