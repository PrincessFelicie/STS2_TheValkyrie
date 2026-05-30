using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class MusicalBridge : TheValkyrieCard
{
    public MusicalBridge() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithVar("Bless", 1); 
        
        WithTip(CustomEnum.Bless);
        WithTips(c => HoverTipFactory.FromEnchantment<Refrain>(c.DynamicVars["Bless"].IntValue));
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        int energySpent = ResolveEnergyXValue();
        if (this.IsUpgraded) 
            energySpent++;
        EnchantmentModel enchantment = ModelDb.Enchantment<Refrain>();
        for (int i = 0; i < energySpent; ++i)
        {
            if (!PileType.Hand.GetPile(this.Owner).Cards.Where(enchantment.CanEnchant).Any())
            {
                ThinkCmd.Play(new LocString("combat_messages", "ENCHANT_NO_TARGET"), Owner.Creature, 2.0);
                break;
            }
            CardModel card = PileType.Hand.GetPile(this.Owner).Cards.Where(enchantment.CanEnchant).TakeRandom(1, Owner.RunState.Rng.CombatCardSelection).First();
            CardCmd.Enchant<Refrain>(card, DynamicVars["Bless"].BaseValue); //Because Refrain is flagged as stackable, CardCmd.Enchant handles either creating the new enchantment or incrementing the enchantment.amount
        }
    }

    protected override void OnUpgrade()
    {
    }
}