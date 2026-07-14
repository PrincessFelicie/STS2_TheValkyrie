using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheValkyrie.TheValkyrieCode.Powers;

namespace TheValkyrie.TheValkyrieCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Smite : TheValkyrieCard
{
    public Smite() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(2, 2);
        WithBlock(2, 2);
        WithPower<OverexertionPower>(2);
        WithKeywords(CardKeyword.Exhaust, CardKeyword.Retain);
        WithTags(CustomEnum.Smite);
    }
    
    public static async Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0 || CombatManager.Instance.IsOverOrEnding)
            return [];
        List<CardModel> smites = [];
        for (int index = 0; index < count; ++index)
            smites.Add(combatState.CreateCard<Smite>(owner));
        await CardPileCmd.AddGeneratedCardsToCombat(smites, PileType.Hand, owner);
        return smites;
    }

    public static async Task<IEnumerable<CardModel>> CreateInHandWithEnchantment<T>(
        Player owner,
        int count,
        int enchantmentCount,
        ICombatState combatState) where T : EnchantmentModel
    {
        return await CreateInHandWithEnchantment(owner, count, ModelDb.Enchantment<T>(), enchantmentCount, combatState);
    }

    public static async Task<IEnumerable<CardModel>> CreateInHandWithEnchantment(
        Player owner,
        int count,
        EnchantmentModel enchantment,
        int enchantmentCount,
        ICombatState combatState)
    {
        if (count == 0 || CombatManager.Instance.IsOverOrEnding)
            return [];
        List<CardModel> smites = [];
        for (int index = 0; index < count; ++index)
        {
            CardModel c = combatState.CreateCard<Smite>(owner);
            smites.Add(c);
            CardCmd.Enchant(enchantment.ToMutable(), c, enchantmentCount);
        }
        await CardPileCmd.AddGeneratedCardsToCombat(smites, PileType.Hand, owner);
        return smites;
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHandBlessed(
        Player owner,
        int smiteCount,
        int enchantmentCount,
        ICombatState combatState)
    {
        if (smiteCount == 0 || CombatManager.Instance.IsOverOrEnding)
            return [];
        List<CardModel> smites = [];
        for (int index = 0; index < smiteCount; ++index)
        {
            CardModel c = combatState.CreateCard<Smite>(owner);
            smites.Add(c);
            await BlessCmd.EnchantOrUpgradeEnchant(c, enchantmentCount);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(smites, PileType.Hand, owner);
        return smites;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<OverexertionPower>(choiceContext, Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}