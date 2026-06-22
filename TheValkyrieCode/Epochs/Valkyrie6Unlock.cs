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
            EpochModel.Get(EpochModel.GetId<Valkyrie1KillBosses>()),
            EpochModel.Get(EpochModel.GetId<Valkyrie2BeatAct1>()),
            EpochModel.Get(EpochModel.GetId<Valkyrie3BeatAct3>()),
            EpochModel.Get(EpochModel.GetId<Valkyrie4KillElites>()),
            EpochModel.Get(EpochModel.GetId<Valkyrie5Ascension1>()),
            EpochModel.Get(EpochModel.GetId<Valkyrie7BeatAct2>())
        ];
    }
    
    public override void QueueUnlocks()
    {
        QueueTimelineExpansion(GetTimelineExpansion());
    }
}