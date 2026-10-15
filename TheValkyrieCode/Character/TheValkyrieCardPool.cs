using BaseLib.Abstracts;
using TheValkyrie.TheValkyrieCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using TheValkyrie.TheValkyrieCode.Cards.ModCompat;
using TheValkyrie.TheValkyrieCode.Cards.ModCompat.AncientsAwakened;
using TheValkyrie.TheValkyrieCode.ModConfiguration;
using TheValkyrie.TheValkyrieCode.Patches.ModCompatPatches.AncientsAwakened;

namespace TheValkyrie.TheValkyrieCode.Character;

public class TheValkyrieCardPool : CustomCardPoolModel
{
    public override string Title => TheValkyrie.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 0.08f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load TheValkyrie/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("923c00");

    public override bool IsColorless => false;

    private CardModel[]? _allCards;
    public override IEnumerable<CardModel> AllCards
    {
        get
        {
            if (this._allCards == null)
            {
                this._allCards = this.GenerateAllCards();
                this._allCards = [.. ModHelper.ConcatModelsFromMods(this, this._allCards)];
            }
            
            if (ValkyrieModConfig.AncientsAwakenedCustomCards && AncientsAwakenedCardRegisterPatch.IsLoaded)
            {
                AncientsAwakenedCardRegisterPatch.RegisterPerfectedDefendForValkyrie(ModelDb.Card<UnchosenDefend>());
                AncientsAwakenedCardRegisterPatch.RegisterPerfectedStrikeForValkyrie(ModelDb.Card<UnchosenStrike>());
                AncientsAwakenedCardRegisterPatch.RegisterExperimentalSerumCardForValkyrie(ModelDb.Card<Contredanse>());
            }
            else
            {
                _allCards = _allCards.Where(c => c is not TheValkyrieAncientsAwakenedCard).ToArray();
            }
            
            return this._allCards;
        }
    }
    
    protected override IEnumerable<CardModel> FilterThroughEpochs(UnlockState unlockState, IEnumerable<CardModel> cards)
    {
        var list = cards.ToList();
        
        var ancientsAwakenedType =
            AccessTools.TypeByName("AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool");
        if (ancientsAwakenedType == null)
        {
            list.RemoveAll(c => c is TheValkyrieAncientsAwakenedCard);
        }

        return list;
    }
}