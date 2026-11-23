using MegaCrit.Sts2.Core.Timeline;

namespace TheValkyrie.TheValkyrieCode.Epochs;

public class Valkyrie6Unlock : EpochModel
{
    public override string Id => "THEVALKYRIE-VALKYRIE_STORY6_UNLOCK";
    public override EpochEra Era => EpochEra.Invitation6;
    public override int EraPosition => 2;
    public override string? StoryId => "ValkyrieTakesOff";
    
    public override EpochModel[] GetTimelineExpansion()
    {
        return
        [
            Get(GetId<Valkyrie1KillBosses>()),
            Get(GetId<Valkyrie2BeatAct1>()),
            Get(GetId<Valkyrie3BeatAct3>()),
            Get(GetId<Valkyrie4KillElites>()),
            Get(GetId<Valkyrie5Ascension1>()),
            Get(GetId<Valkyrie7BeatAct2>())
        ];
    }
    
    public override void QueueUnlocks()
    {
        QueueTimelineExpansion(GetTimelineExpansion());
    }
}