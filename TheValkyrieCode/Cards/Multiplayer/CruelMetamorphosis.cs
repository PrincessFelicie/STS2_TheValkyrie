using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheValkyrie.TheValkyrieCode.Cards.Multiplayer;

public class CruelMetamorphosis : TheValkyrieCard
{
    public CruelMetamorphosis() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
    {
        WithUpgradingCardTip<GodByrd>();
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        if (play.Target?.Player != null && CombatState != null)
        {
            foreach (CardModel original in PileType.Hand.GetPile(play.Target.Player).Cards.Where(c => c.IsTransformable).ToList())
            {
                CardModel card = CombatState.CreateCard<GodByrd>(play.Target.Player);
                if (IsUpgraded)
                    CardCmd.Upgrade(card);
                await CardCmd.Transform(original, card);
            }
        }
    }

    protected override void OnUpgrade()
    {
    }
}