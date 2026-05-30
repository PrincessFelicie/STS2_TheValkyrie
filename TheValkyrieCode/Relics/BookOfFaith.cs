using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Cards.Token;

namespace TheValkyrie.TheValkyrieCode.Relics;

public class BookOfFaith : TheValkyrieRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Smites", 3)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get => HoverTipFactory.FromCardWithCardHoverTips<Smite>();
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber > 1)
            return;
        this.Flash();
        await Smite.CreateInHand(Owner, DynamicVars["Smites"].IntValue, combatState);
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<BookOfOrnithology>();
    }

    public override async Task BeforeCombatStart()
    {
        var combatState = this.Owner.Creature.CombatState;
        if (combatState == null) return;
        if (combatState.ContainsMonster<CalcifiedCultist>() || combatState.ContainsMonster<DampCultist>() ||
            combatState.ContainsMonster<DevotedSculptor>())
        {
            TalkCmd.Play(new LocString("combat_messages", "FIGHTING_CULTIST"), Owner.Creature, VfxColor.Orange, VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
            if (combatState.ContainsMonster<CalcifiedCultist>())
            {
                TalkCmd.Play(new LocString("combat_messages", "CALCIFIED_CULTIST_REPLY"),
                    combatState.Enemies.First(creature => creature.Monster is CalcifiedCultist), VfxColor.Blue, VfxDuration.Standard);
                await Cmd.CustomScaledWait(0.4f, 1);
            }
            if (combatState.ContainsMonster<DampCultist>()) TalkCmd.Play(new LocString("combat_messages", "DAMP_CULTIST_REPLY"), combatState.Enemies.First(creature => creature.Monster is DampCultist), VfxColor.Blue, VfxDuration.Standard);
            if (combatState.ContainsMonster<DevotedSculptor>()) TalkCmd.Play(new LocString("combat_messages", "DEVOTED_SCULPTOR_REPLY"), combatState.Enemies.First(creature => creature.Monster is DevotedSculptor), VfxColor.Blue, VfxDuration.VeryLong);
        }
        return;
    }
}