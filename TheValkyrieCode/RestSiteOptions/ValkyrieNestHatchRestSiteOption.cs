using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Extensions;
using TheValkyrie.TheValkyrieCode.Relics;
using TheValkyrie.TheValkyrieCode.Relics.NestBirdpyps;

namespace TheValkyrie.TheValkyrieCode.RestSiteOptions;

public class ValkyrieNestHatchRestSiteOption(Player owner): CustomRestSiteOption(owner)
{
    public override string? CustomIconPath => "/ui/rest_site/option_valkyrie_nest_hatch.png".ImagePath();

    public override LocString Description
    {
        get
        {
            LocString description = base.Description;
            int timesHatched = Owner.GetRelic<ByrdNest>()?.TimesHatched ?? 0;
            int variable = 3 - timesHatched;
            description.Add("HatchesLeft", variable);
            int variable2 = timesHatched + 1;
            description.Add("EggNumber", variable2);
            return description;
        }
    }
    
    public override string OptionId => "VALKYRIE_NEST_HATCH";
    
    public override async Task<bool> OnSelect()
    {
        ByrdNest? relic = Owner.GetRelic<ByrdNest>();
        if (relic == null)
            return await Task.FromResult(false);
        relic.TimesHatched+=1;
        if (relic.TimesHatched == 1)
        {
            await RelicCmd.Obtain<YellowByrdpip>(Owner);
        }
        else if (relic.TimesHatched == 2)
        {
            await RelicCmd.Obtain<RedByrdpip>(Owner);
        }
        else if (relic.TimesHatched >= 3)
        {
            await RelicCmd.Obtain<BlueByrdpip>(Owner);
        }
        return await Task.FromResult(true);
    }
    
    public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
    {
        SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
        return Task.CompletedTask;
    }

    public override Task DoRemotePostSelectVfx()
    {
        SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
        NRestSiteRoom? instance = NRestSiteRoom.Instance;
        NRestSiteCharacter? parent = instance?.Characters.First(c => c.Player == Owner);
        NRelicFlashVfx? child = NRelicFlashVfx.Create(ModelDb.Relic<BlueByrdpip>());
        if (child == null)
            return Task.CompletedTask;
        if (parent != null)
            parent.AddChildSafely(child);
        child.Position = Vector2.Zero;
        return Task.CompletedTask;
    }
}