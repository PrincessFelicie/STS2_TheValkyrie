using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using TheValkyrie.TheValkyrieCode.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheValkyrie.TheValkyrieCode.Cards.Token;
using TheValkyrie.TheValkyrieCode.Enchantments;

namespace TheValkyrie.TheValkyrieCode.Cards.Rare;

public class DesperateHour : TheValkyrieCard
{
    public DesperateHour() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Smite));
        WithVar("Bless", 1); WithTip(CustomEnum.Bless);
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> list1 = PileType.Hand.GetPile(this.Owner).Cards.ToList();
        int exhaustCount = list1.Count;
        foreach (CardModel card in list1)
            await CardCmd.Exhaust(choiceContext, card);
        
        List<EnchantmentModel> validEnchantments = new List<EnchantmentModel>();
        validEnchantments.Add(ModelDb.Enchantment<Sharp>());
        validEnchantments.Add(ModelDb.Enchantment<Nimble>());
        validEnchantments.Add(ModelDb.Enchantment<Sanguine>());
        validEnchantments.Add(ModelDb.Enchantment<Aegis>());

        
        int enchantmentCount = 1;
        
        for (int i = 0; i < exhaustCount ; ++i)
        {
            EnchantmentModel enchantment = validEnchantments.TakeRandom(1, Owner.RunState.Rng.CombatCardGeneration).First();
            switch (enchantment.CanonicalInstance)
            {
                case Sharp:
                    enchantmentCount = 3;
                    break;
                case Nimble:
                case Sanguine:
                    enchantmentCount = 2;
                    break;
                case Aegis:
                    enchantmentCount = 1;
                    break;
            }
            if (CombatState == null) return;
            await Smite.CreateInHandWithEnchantment(Owner, 1, enchantment, enchantmentCount, CombatState);
            await Cmd.Wait(0.1f);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}