using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class HopeInDespair : TheValkyrieCard
{
    public HopeInDespair() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(12, 6);
        WithVar("HopeInDespairPower", 1);
    }
    
    protected override bool ShouldGlowGoldInternal => IsOnlyCardInHand;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (IsOnlyCardInHand)
        {
            await CommonActions.CardBlock(this, play);
            await PowerCmd.Apply<HopeInDespairPower>(choiceContext, Owner.Creature, DynamicVars["HopeInDespairPower"].IntValue, Owner.Creature, this);
        }
    }
    
    private bool IsOnlyCardInHand => !PileType.Hand.GetPile(this.Owner).Cards.Except<CardModel>([this]).Any();

    protected override void OnUpgrade()
    {
    }
}