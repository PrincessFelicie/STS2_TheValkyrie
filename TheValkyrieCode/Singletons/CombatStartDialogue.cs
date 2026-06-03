using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Singletons;

public class CombatStartDialogue() : CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (combatState.RoundNumber > 1 || side != CombatSide.Player) return; //if it's not player turn 1, return
        
        Player? valkyrie = combatState.Players.FirstOrDefault(p => p.Character is Character.TheValkyrie);
        if (valkyrie == null) return; //if there's no valkyrie, return

        if (!combatState.ContainsMonster<CalcifiedCultist>() && !combatState.ContainsMonster<DampCultist>() &&
            !combatState.ContainsMonster<DevotedSculptor>()) return; //if there's no cultist, return
        
        TalkCmd.Play(
            valkyrie.GetRelic<BookOfOrnithology>() != null
                ? new LocString("combat_messages", "FIGHTING_CULTIST_UNSTOPPABLE")
                : new LocString("combat_messages", "FIGHTING_CULTIST"), 
            valkyrie.Creature, VfxColor.Orange, VfxDuration.Standard); //if the first valkyrie has book of ornithology, change the line, otherwise use the regular line

        await Cmd.CustomScaledWait(0.4f, 1);
            
        if (combatState.ContainsMonster<CalcifiedCultist>())
        {
            TalkCmd.Play(new LocString("combat_messages", "CALCIFIED_CULTIST_REPLY"),
                combatState.Enemies.First(creature => creature.Monster is CalcifiedCultist), VfxColor.Blue, VfxDuration.Standard);
            await Cmd.CustomScaledWait(0.4f, 1);
        }
        
        if (combatState.ContainsMonster<DampCultist>()) 
            TalkCmd.Play(new LocString("combat_messages", "DAMP_CULTIST_REPLY"), combatState.Enemies.First(creature => creature.Monster is DampCultist), VfxColor.Blue, VfxDuration.Standard);
        
        if (combatState.ContainsMonster<DevotedSculptor>()) 
            TalkCmd.Play(new LocString("combat_messages", "DEVOTED_SCULPTOR_REPLY"), combatState.Enemies.First(creature => creature.Monster is DevotedSculptor), VfxColor.Blue, VfxDuration.VeryLong);
    }
}