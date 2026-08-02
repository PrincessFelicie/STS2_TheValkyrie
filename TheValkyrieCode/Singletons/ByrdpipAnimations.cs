using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Cards.Rare;
using TheValkyrie.TheValkyrieCode.Pets;

namespace TheValkyrie.TheValkyrieCode.Singletons;

public class ByrdpipAnimations() : CustomSingletonModel(HookType.Combat)
{
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Player.PlayerCombatState == null) return;
        if (cardPlay.Card is ByrdSwoop)
        {
            Creature? pet = cardPlay.Player.PlayerCombatState.GetPet<YellowByrdpipPet>();
            if (pet != null)
            {
                await CreatureCmd.TriggerAnim(pet, "Attack", 0);
                //no need for sfx because it's already present on ByrdSwoop itself
            }
        }

        if (cardPlay.Card is Peck)
        {
            Creature? pet = cardPlay.Player.PlayerCombatState.GetPet<RedByrdpipPet>();
            if (pet != null)
            {
                await CreatureCmd.TriggerAnim(pet, "Attack", 0);
                SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
            }
        }

        if (cardPlay.Card is TerritorialInstincts)
        {
            Creature? pet = cardPlay.Player.PlayerCombatState.GetPet<BlueByrdpipPet>();
            if (pet != null)
            {
                await CreatureCmd.TriggerAnim(pet, "Attack", 0);
                SfxCmd.Play("event:/sfx/enemy/enemy_attacks/byrdonis/byrdonis_get_angry");
            }
        }
    }
}