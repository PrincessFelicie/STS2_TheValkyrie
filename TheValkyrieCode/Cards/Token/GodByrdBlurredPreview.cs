using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheValkyrie.TheValkyrieCode.Cards.Token;

[Pool(typeof(EventCardPool))]
public class GodByrdBlurredPreview : TheValkyrieCard
{
    public GodByrdBlurredPreview() : base(0, CardType.Skill, CardRarity.Event, TargetType.None, false)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        //this card should never be obtainable
    }

    protected override void OnUpgrade()
    {
    }
}