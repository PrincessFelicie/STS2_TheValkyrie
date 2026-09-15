using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheValkyrie.TheValkyrieCode.Utilities;

public class CustomCardAnimCmd
{
    public static async Task GrowAndShrink(NCard card, float mult = 1.5f)
    {
        Tween tween = card.CreateTween();
        Vector2 savedSize = card.Scale;
        tween.TweenProperty(card, "scale", savedSize*mult, 0.25f);
        tween.TweenProperty(card, "scale", savedSize, 0.25f);
    }
}