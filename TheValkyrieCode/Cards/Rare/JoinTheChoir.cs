using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Enchantments;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class JoinTheChoir : TheValkyrieCard
{
    public JoinTheChoir() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
    {
        WithVar("Bless", 1);
        WithTip(CustomEnum.Bless);

        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        
        WithTips(c => HoverTipFactory.FromEnchantment<Refrain>(c.DynamicVars["Bless"].IntValue));
    }
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState == null) return;
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        
        EnchantmentModel enchantment = ModelDb.Enchantment<Refrain>();
        foreach (Creature creature in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer))
        {
            Player teammate = creature.Player ?? throw new InvalidOperationException();
            if (!PileType.Hand.GetPile(teammate).Cards.Where(enchantment.CanEnchant).Any())
            {
                //ThinkCmd.Play(new LocString("combat_messages", "ENCHANT_NO_TARGET"), Owner.Creature, 2.0);
                continue;
            }
            CardModel card = PileType.Hand.GetPile(teammate).Cards.Where(enchantment.CanEnchant).TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).First();
            CardCmd.Enchant<Refrain>(card, DynamicVars["Bless"].BaseValue);
        }
        
        this.EnergyCost.AddThisCombat(1);
    }

    protected override void OnUpgrade()
    {
    }
}