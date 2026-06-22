using MegaCrit.Sts2.Core.Timeline;

namespace TheValkyrie.TheValkyrieCode.Epochs;


//After reflection, I have decided that implementing epochs is a mess right now, and I'll wait for baselib to support custom epochs instead. I'll leave this preliminary work in for now, but it's unused.
public class ValkyrieStory : StoryModel
{
    protected override string Id => "VALKYRIE";

    public override EpochModel[] Epochs =>
    [
        EpochModel.Get(EpochModel.GetId<Valkyrie1KillBosses>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie2BeatAct1>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie3BeatAct3>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie4KillElites>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie5Ascension1>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie6Unlock>()),
        EpochModel.Get(EpochModel.GetId<Valkyrie7BeatAct2>()),
    ];
}