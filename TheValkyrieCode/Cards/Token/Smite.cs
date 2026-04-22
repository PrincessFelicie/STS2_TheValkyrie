using BaseLib.Patches.Content;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using TheValkyrie.TheValkyrieCode.Cards;
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
    
    public static async Task<CardModel?> CreateInHand(Player owner, CombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault<CardModel>();
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        CombatState combatState)
    {
        if (count == 0)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        List<CardModel> smites = new List<CardModel>();
        for (int index = 0; index < count; ++index)
            smites.Add((CardModel) combatState.CreateCard<Smite>(owner));
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) smites, PileType.Hand, true);
        return (IEnumerable<CardModel>) smites;
    }

    public static async Task<IEnumerable<CardModel>> CreateInHandWithEnchantment<T>(
        Player owner,
        int count,
        int enchantmentCount,
        CombatState combatState) where T : EnchantmentModel
    {
        return (await CreateInHandWithEnchantment(owner, count, ModelDb.Enchantment<T>(), enchantmentCount, combatState));
    }

    public static async Task<IEnumerable<CardModel>> CreateInHandWithEnchantment(
        Player owner,
        int count,
        EnchantmentModel enchantment,
        int enchantmentCount,
        CombatState combatState)
    {
        if (count == 0)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        List<CardModel> smites = new List<CardModel>();
        for (int index = 0; index < count; ++index)
        {
            CardModel c = combatState.CreateCard<Smite>(owner);
            smites.Add(c);
            CardCmd.Enchant(enchantment.ToMutable(), c, enchantmentCount);
        }

        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) smites, PileType.Hand, true);
        return (IEnumerable<CardModel>) smites;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target).Execute(choiceContext);
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<OverexertionPower>(Owner.Creature, DynamicVars["OverexertionPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
    }
}