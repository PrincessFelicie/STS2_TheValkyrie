using BaseLib.Abstracts;
using BaseLib.Utils;
using TheValkyrie.TheValkyrieCode.Character;

namespace TheValkyrie.TheValkyrieCode.Potions;

[Pool(typeof(TheValkyriePotionPool))]
public abstract class TheValkyriePotion : CustomPotionModel;