using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class RousingDitty : TheValkyrieRelic
{
    //todo missing a relic image
    
    public override RelicRarity Rarity => RelicRarity.Event;

    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
            new BlockVar(1M, ValueProp.Unpowered)
        ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 || cardPlay.Card.Owner != Owner)
            return;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null, true);
    }
}