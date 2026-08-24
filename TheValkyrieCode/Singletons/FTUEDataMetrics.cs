using BaseLib.Abstracts;
using BaseLib.Config;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using TheValkyrie.TheValkyrieCode.ModConfiguration;
using TheValkyrie.TheValkyrieCode.Relics;

namespace TheValkyrie.TheValkyrieCode.Singletons;

public class FTUEDataMetrics() : CustomSingletonModel(HookType.Run)
{
    public override async Task BeforeCombatStart()
    {
        RunState?
            runState = RunManager.Instance
                .DebugOnlyGetState(); // todo replace this with publicizing RunManager.State if/when this gets deprecated

        if (ValkyrieModConfig.MetricsDataShare) //if data share is on...
        {
            if (ValkyrieModConfig.MetricsDataSharePopupSeen) //if the FTUE is already on seen...
                return; //we can stop there :)

            ValkyrieModConfig.MetricsDataSharePopupSeen = true; //otherwise flip the FTUE as seen
            ModConfig.SaveDebounced<ValkyrieModConfig>();

            return; //and we can stop there :)
        }

        if (!ValkyrieModConfig.MetricsDataSharePopupSeen &&
            runState.Players.Any(p =>
                p.Character is Character
                    .TheValkyrie)) //if datashare ISN'T on, FTUE has NOT been seen, and Valkyrie is present...
        {
            NGenericPopup? shareRequest = NGenericPopup.Create();
            if (shareRequest == null) return;
            
            NModalContainer.Instance?.Add(shareRequest);

            bool selection = await shareRequest.WaitForConfirmation(
                new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_POPUP.body"),
                new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_POPUP.header"),
                new LocString("main_menu_ui", "GENERIC_POPUP.cancel"),
                new LocString("main_menu_ui", "GENERIC_POPUP.confirm"));
            
            NModalContainer.Instance?.Clear();
            await Cmd.Wait(0.000001f); //LOAD BEARING!!! IF YOU REMOVE IT, THE SEQUENCED POPUPS LOCK UP!!!

            if (selection)
            {
                await ActivateDataShare();
            }
            else
            {
                await TurnOffDataShare();
            }
        }
    }

    private static async Task ActivateDataShare()
    {
        ValkyrieModConfig.MetricsDataShare = true;
        ValkyrieModConfig.MetricsDataSharePopupSeen = true;
        ModConfig.SaveDebounced<ValkyrieModConfig>();

        NGenericPopup? thankYouMessage = NGenericPopup.Create();
        if (thankYouMessage == null) return;
                
        NModalContainer.Instance?.Add(thankYouMessage);
        await thankYouMessage.WaitForConfirmation(
            new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_THANK_YOU.body"),
            new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_THANK_YOU.header"),
            null,
            new LocString("main_menu_ui", "GENERIC_POPUP.ok"));
        
        await Cmd.Wait(0.000001f); //Safety wait in case other mods copy-paste this code.
    }
    
    private static async Task TurnOffDataShare()
    {
        ValkyrieModConfig.MetricsDataSharePopupSeen = true;
        ModConfig.SaveDebounced<ValkyrieModConfig>();

        NGenericPopup? NoWorriesMessage = NGenericPopup.Create();
        if (NoWorriesMessage == null) return;
                
        NModalContainer.Instance?.Add(NoWorriesMessage);
        await NoWorriesMessage.WaitForConfirmation(
            new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_NO_WORRIES.body"),
            new LocString("main_menu_ui", "VALKYRIE_METRICS_FTUE_NO_WORRIES.header"),
            null,
            new LocString("main_menu_ui", "GENERIC_POPUP.ok"));
        
        await Cmd.Wait(0.000001f); //Safety wait in case other mods copy-paste this code.
    }
}