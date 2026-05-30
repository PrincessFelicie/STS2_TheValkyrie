using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Uncommon;

public class Overprepare : TheValkyrieCard
{
    public Overprepare() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        //put a card from your hand at the bottom of your draw pile.
        WithPower<VulnerablePower>(1, 1);
    }

    private IEnumerable<CardModel> _selectedCards = new List<CardModel>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardSelectorPrefs prefs = new (SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card == null) return;
        await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Bottom);
        _selectedCards = _selectedCards.Append(card);
    }
    
    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (!_selectedCards.Contains(card) || CombatState == null)
            return;
            
        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), enemy, DynamicVars["VulnerablePower"].IntValue, Owner.Creature,
                this);
        }
        _selectedCards = _selectedCards.Except([card]);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}