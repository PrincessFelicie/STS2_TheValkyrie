using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace TheValkyrie.TheValkyrieCode.Utilities;

//thank you to JohnnyBazooka89 for this piece of code!

public static class RelicIconUpdater
{
    public static void ReloadRelicIcon(ModelId relicId)
    {
        foreach (NRelicInventoryHolder nRelicInventoryHolder in NRun.Instance?.GlobalUi.RelicInventory.RelicNodes ?? [])
        {
            var relic = nRelicInventoryHolder.Relic;
            if (relic.Model.Id == relicId)
            {
                relic.Reload();
            }
        }
    }
}