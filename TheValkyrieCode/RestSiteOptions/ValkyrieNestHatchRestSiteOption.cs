using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheValkyrie.TheValkyrieCode.Relics;
using TheValkyrie.TheValkyrieCode.Relics.NestBirdpyps;

namespace TheValkyrie.TheValkyrieCode.RestSiteOptions;

public class ValkyrieNestHatchRestSiteOption(Player owner): RestSiteOption(owner)
{
    public override LocString Description
    {
        get
        {
            LocString description = base.Description;
            int timesHatched = this.Owner.GetRelic<ByrdNest>()?.TimesHatched ?? 0;
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
        ByrdNest? relic = this.Owner.GetRelic<ByrdNest>();
        if (relic == null)
            return await Task.FromResult<bool>(false);
        relic.TimesHatched+=1;
        if (relic.TimesHatched == 1)
        {
            await RelicCmd.Obtain<YellowByrdpip>(this.Owner);
        }
        else if (relic.TimesHatched == 2)
        {
            await RelicCmd.Obtain<RedByrdpip>(this.Owner);
        }
        else if (relic.TimesHatched >= 3)
        {
            await RelicCmd.Obtain<BlueByrdpip>(this.Owner);
        }
        return await Task.FromResult<bool>(true);
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
        NRestSiteCharacter? parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : null;
        NRelicFlashVfx? child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<Byrdpip>());
        if (child == null)
            return Task.CompletedTask;
        if (parent != null)
            parent.AddChildSafely((Node) child);
        child.Position = Vector2.Zero;
        return Task.CompletedTask;
    }
}