using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class Blunderbuss : TheValkyrieCard
{
    public Blunderbuss() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(Smite));
        WithPower<FrailPower>(99);
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<FrailPower>(choiceContext, Owner.Creature,DynamicVars["FrailPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<BlunderbussPower>(choiceContext, Owner.Creature,1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}